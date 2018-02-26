using UnityEngine;
using UnityEditor;

public class CheckImports : AssetPostprocessor {
	bool fixMipMap = true;
	bool fixForceToMono = true;

	bool fixReadable = false; //be careful!

	public void OnPreprocessTexture() {
		TextureImporter importer = (TextureImporter)assetImporter;

		if(importer.isReadable) {
			if (fixReadable) {
				importer.isReadable = false;
            	importer.SaveAndReimport();
				Debug.LogWarning("Texture auto uncheck isReadable:"+importer.assetPath);
			}else {
				Debug.LogWarning("Texture isReadable:"+importer.assetPath);
			}
        }

		if (importer.mipmapEnabled) {
			if (fixMipMap) {
				importer.mipmapEnabled = false;
            	importer.SaveAndReimport();
				Debug.Log("Texture auto uncheck mipmap:" + importer.assetPath);
			}else {
				Debug.LogWarning("Texture mipmap is checked:"+importer.assetPath);
			}
		}
	}

	public void OnPreprocessAudio() {
		AudioImporter importer = (AudioImporter)assetImporter;

		if(!importer.forceToMono) {
			if (fixForceToMono) {
				importer.forceToMono = true;
				importer.SaveAndReimport();

				Debug.Log("Audio auto check forceToMono:" + importer.assetPath);
			}else {
				Debug.LogWarning("Audio forceToMono is unchecked:"+importer.assetPath);
			}
		}
	}

	public void OnPreprocessModel() {
        ModelImporter importer = (ModelImporter)assetImporter;

        if(importer.isReadable) {
			if (fixReadable) {
				importer.isReadable = false;
            	importer.SaveAndReimport();
				Debug.LogWarning("Model auto uncheck isReadable:"+importer.assetPath);
			}else {
				Debug.LogWarning("Model isReadable:"+importer.assetPath);
			}
        }

		if (ModelImporterMeshCompression.Off == importer.meshCompression) {
			Debug.LogWarning("Model ModelImporterMeshCompression is Off:"+importer.assetPath);
		}
    }
	
}
