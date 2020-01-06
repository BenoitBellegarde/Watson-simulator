
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

uniform FP3 ambientColorBottom;
uniform FP3 ambientColorHorizon;
uniform FP3 ambientColorTop;
uniform FP4 ambientShAr;
uniform FP4 ambientShAg;
uniform FP4 ambientShAb;
uniform FP4 ambientShBr;
uniform FP4 ambientShBg;
uniform FP4 ambientShBb;
uniform FP4 ambientShC;
uniform uint ambientMode;

FP3 ComputeAmbientLighting(FP4 viewDirection, FP scattering)
{
    FP3 ambientLight;

    if (ambientMode == 3) // Flat ambient
    {
        ambientLight = ambientColorHorizon;
    }
    else if (ambientMode == 1) // Gradient ambient
    {
        FP gradient = viewDirection.y;
        FP3 color = lerp(ambientColorBottom, ambientColorHorizon, saturate(gradient + 1.0f)); // bottom to horizon
        color = lerp(color, ambientColorTop, saturate(gradient));								// horizon to top
		
		FP3 meanColor = (ambientColorBottom + ambientColorHorizon + ambientColorTop) * 1.0f / 3.0f;	// does it work? yes
		color = lerp(color, meanColor, scattering);														// is it mathematically correct? probably not
		
        ambientLight = color;
    }
	else // Skybox ambient
    {
        viewDirection.xyz *= scattering; // Henyey-Greenstein phase function (https://bartwronski.files.wordpress.com/2014/08/bwronski_volumetric_fog_siggraph2014.pdf#page=55)
        ambientLight = EvaluateSphericalHarmonics(viewDirection, ambientShAr, ambientShAg, ambientShAb, ambientShBr, ambientShBg, ambientShBb, ambientShC);
    }
    
    return ambientLight;
}

