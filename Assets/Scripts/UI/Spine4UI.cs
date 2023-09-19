using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Spine.Unity;

public class Spine4UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    SkeletonGraphic sg;
    public void Load(string char_name = "Kiki")
    {
        ResourceManager.Instance.LoadAsync<SkeletonDataAsset>("Assets/Res/Spine/" + char_name + "/" + char_name + "_SkeletonData.asset", (obj) =>
        {
            SkeletonDataAsset skeletonDataAsset = obj as SkeletonDataAsset;
            ResourceManager.Instance.LoadAsync<Material>("Assets/Res/Spine/" + char_name + "/" + char_name + "_Material.mat", (mat) =>
            {
                Material skeletonGraphicMaterial = mat as Material;
                sg = SkeletonGraphic.NewSkeletonGraphicGameObject(skeletonDataAsset, this.transform, skeletonGraphicMaterial);
                sg.Initialize(false);
                sg.Skeleton.SetSlotsToSetupPose();
                //sg.AnimationState.SetAnimation(0, "1happy", true);
            });
        });
    }

    public void PlayBySenti(int sentiValue)
    {
        if (sg == null)
            return;
        switch(sentiValue)
        {
            case 1:
                sg.AnimationState.SetAnimation(0, "2fear", true);
                break;
            case 2:
                sg.AnimationState.SetAnimation(0, "3angle", true);
                break;
            case 3:
                sg.AnimationState.SetAnimation(0, "4lose", false);
                break;
            case 4:
                sg.AnimationState.SetAnimation(0, "5inquisitive", false);
                break;
            case 5:
                sg.AnimationState.SetAnimation(0, "6banter", false);
                break;
            default:
                sg.AnimationState.SetAnimation(0, "1happy", false);
                break;
        }
    }


}
