using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizableElement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;


    [SerializeField]
    private List<PositionedSprite> _spriteOption;

    [field: SerializeField]
    public int SpriteIndex { get; private set; }

    [SerializeField]
    private List<Color> _colorOptions;


    [field: SerializeField]
    public int ColorIndex;

    [SerializeField]
    private List<SpriteRenderer> _copyColorTo;

    //ganti warna selanjutnya
    [ContextMenu("Next Sprite")]
    public PositionedSprite NextSprite()
    {
        SpriteIndex = Mathf.Min(SpriteIndex + 1, _spriteOption.Count - 1);
        UpdateSprite();
        return _spriteOption[SpriteIndex];
    }
    //ganti sprite sebulumnya
    [ContextMenu("Previous Sprite")]
     public PositionedSprite PreviousSprite()
    {
        SpriteIndex = Mathf.Max(SpriteIndex - 1, 0);
        UpdateSprite();
        return _spriteOption[SpriteIndex]; 

    }
    [ContextMenu("Next Color")]
    //buat ganti warna selanjutnya
    public Color NextColor()
    {
       ColorIndex = Mathf.Min(ColorIndex + 1, _colorOptions.Count - 1);
        UpdateColor();
        return _colorOptions[ColorIndex];
    }
    //buat ganti warna sebelumnya
    [ContextMenu("Previous Color")]
    public Color PreviousColor()
    {
        ColorIndex = Mathf.Max(ColorIndex - 1, 0);
        UpdateColor();
        return _colorOptions[ColorIndex];
    }
    [ContextMenu("Randomize")]
    public void Randomize()
    {
        SpriteIndex = UnityEngine.Random.Range(0, _spriteOption.Count -1);
        ColorIndex = UnityEngine.Random.Range(0, _colorOptions.Count -1);
        UpdateSprite();
        UpdateColor();

    }
    [ContextMenu("Update Position Moidifier")]
    public void UpdateSpritePositionModifier()
    {
        _spriteOption[SpriteIndex].PositionModifier = transform.localPosition;
    }
    //update sprite selanjutnya/sebelumnya , pilihan spritenya , ngatur sprite renderernya , sama posisi spritenya
    private void UpdateSprite()
    {  
        if (_spriteOption.Count == 0) return;
        SpriteIndex = Mathf.Clamp(SpriteIndex, 0, _spriteOption.Count - 1);
       var positionedSprite = _spriteOption[SpriteIndex];
       _spriteRenderer.sprite = positionedSprite.Sprite;
        transform.localPosition = positionedSprite.PositionModifier;
    }
    private void UpdateColor()
    {
        if (_colorOptions.Count == 0) return;
        ColorIndex = Mathf.Clamp(ColorIndex, 0, _colorOptions.Count - 1);
        var newcolor = _colorOptions[ColorIndex];
        _spriteRenderer.color = newcolor;
        _copyColorTo.ForEach(SpriteRenderer =>  SpriteRenderer.color = newcolor);
        
    }
}
