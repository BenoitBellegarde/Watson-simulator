
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

//-----------------------------------------------------------------------------------------
//			SHEvalLinear
//			Evaluates the spherical harmonics bands
//-----------------------------------------------------------------------------------------
//From UnityCG.cginc
// viewDirection should be normalized, w=1.0
FP3 SHEvalLinearL0L1(FP4 viewDirection, FP4 shAr, FP4 shAg, FP4 shAb)
{
    // Linear (L1) + constant (L0) polynomial terms
    FP3 color;
    color.x = dot(shAr, viewDirection);
    color.y = dot(shAg, viewDirection);
    color.z = dot(shAb, viewDirection);

    return color;
}
// viewDirection should be normalized, w=1.0
FP3 SHEvalLinearL2(FP4 viewDirection, FP4 shBr, FP4 shBg, FP4 shBb, FP4 shC)
{
    // 4 of the quadratic (L2) polynomials
    FP4 v = viewDirection.xyzz * viewDirection.yzzx;
    FP3 color;
    color.r = dot(shBr, v);
    color.g = dot(shBg, v);
    color.b = dot(shBb, v);
	
    // Final (5th) quadratic (L2) polynomial
    FP vC = viewDirection.x * viewDirection.x - viewDirection.y * viewDirection.y;
    color += shC.rgb * vC;

    return color;
}

//-----------------------------------------------------------------------------------------
//			EvaluateSphericalHarmonics
//			Computes the whole spherical harmonics
//-----------------------------------------------------------------------------------------
//From UnityCG.cginc
// viewDirection should be normalized, w=1.0
FP3 EvaluateSphericalHarmonics(FP4 viewDirection, FP4 shAr, FP4 shAg, FP4 shAb, FP4 shBr, FP4 shBg, FP4 shBb, FP4 shC)
{
	// Linear + constant polynomial terms
    FP3 color = SHEvalLinearL0L1(viewDirection, shAr, shAg, shAb);
	// Quadratic polynomials
    color += SHEvalLinearL2(viewDirection, shBr, shBg, shBb, shC);
    return color;
}