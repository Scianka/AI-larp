using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtons : MonoBehaviour
{
    [Header("Looks")]
    public Image wish_button;
    public Sprite WB_default;
    public Sprite WB_light;
    public Image spell_button;
    public Sprite SB_default;
    public Sprite SB_light;

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) DungeonWish();
        if (Input.GetKeyDown(KeyCode.F3)) DungeonSpell();
    }

    private void DungeonWish()
    {
        StartCoroutine("WBLight");
    }

    private void DungeonSpell()
    {
        StartCoroutine("SBLight");
    }

    private IEnumerator WBLight()
    {
        wish_button.sprite = WB_light;
        yield return new WaitForSeconds(0.2f);
        wish_button.sprite = WB_default;
    }

    private IEnumerator SBLight()
    {
        spell_button.sprite = SB_light;
        yield return new WaitForSeconds(0.2f);
        spell_button.sprite = SB_default;
    }
}
