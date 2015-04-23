Shader "MotorShader/LineShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags 
		{ 
			"Queue" = "Transparent+1000"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent" 
		}
		LOD 200
		
		Pass  
      {           
         SetTexture [_MainTex] {combine texture}  
      }
	} 
	FallBack "Diffuse"
}
