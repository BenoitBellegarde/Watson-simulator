
/***************************************************************************
*                                                                          *
*  Copyright (c) Raphaël Ernaelsten (@RaphErnaelsten)                      *
*  All Rights Reserved.                                                    *
*                                                                          *
*  NOTICE: Aura 2 is a commercial project.                                 * 
*  All information contained herein is, and remains the property of        *
*  Raphaël Ernaelsten.                                                     *
*  The intellectual and technical concepts contained herein are            *
*  proprietary to Raphaël Ernaelsten and are protected by copyright laws.  *
*  Dissemination of this information or reproduction of this material      *
*  is strictly forbidden.                                                  *
*                                                                          *
***************************************************************************/

// https://en.wikipedia.org/wiki/Absorbance#Attenuation_coefficient
FP LinearAbsorbance(FP traveledDistance, FP absorbanceFactor)
{
	return traveledDistance * absorbanceFactor;
}

///https://en.wikipedia.org/wiki/Optical_depth#Optical_depth
FP OpticalDepth(FP absorbance)
{
    return absorbance; // * log(10); We can bypass log(10) as it is a constant
}

// https://en.wikipedia.org/wiki/Transmittance#Beer-Lambert_law
FP Transmittance(FP opticalDepth)
{
	return saturate(exp(-opticalDepth));
}

// https://en.wikipedia.org/wiki/Opacity_(optics)#Quantitative_definition
FP Opacity(FP transmitance)
{
	return 1.0f - transmitance;
}

FP GetTransmittance(FP density, FP traveledDistance, FP extinction)
{
	FP absorbance = LinearAbsorbance(traveledDistance, extinction);
	FP opticalDepth = OpticalDepth(absorbance);
	FP transmittance = Transmittance(opticalDepth * density);

	return transmittance;
}

FP GetOpacity(FP density, FP traveledDistance, FP extinction)
{
	FP transmittance = GetTransmittance(density, traveledDistance, extinction);

	return Opacity(transmittance);
}

FP AccumulateTransmittance(FP transmittanceA, FP transmittanceB)
{
	return transmittanceA * transmittanceB; // x^(a+b) = x^a * x^b
}

FP4 AccumulateFog(FP4 colorAndDensityFront, FP4 colorAndDensityBack, FP traveledDistance, FP extinction)
{
	FP transmittance = GetTransmittance(colorAndDensityBack.w, traveledDistance, extinction);
	FP4 accumulatedLightAndTransmittance = FP4(colorAndDensityFront.xyz + colorAndDensityBack.xyz * Opacity(transmittance) * colorAndDensityFront.w, AccumulateTransmittance(transmittance, colorAndDensityFront.w));

	return accumulatedLightAndTransmittance;
}

void ApplyFog(inout FP3 sceneColor, FP3 fogColor, FP density, FP traveledDistance, FP extinction)
{
	FP gradient = GetOpacity(density, traveledDistance, extinction);
	sceneColor = lerp(sceneColor, fogColor, gradient);
}