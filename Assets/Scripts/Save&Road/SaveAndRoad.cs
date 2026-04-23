using System.Numerics;

[System.Serializable]
public class SaveAndLoad
{
    // [설정값]
    public float MasterVolume; // 배경음
    public float Brightness; // 밝기
    public float EffectSound; // 이펙트소리
    public float Dubbing; // 더빙

    // [시나리오 상태]
    public int CurrentID; // 저장당시 ID:[N] 값
    public string SpeakingName; // 나.한서아 등등
    public string DialogueText; // 대사창
    public int NextIndexOverride;// ID고정값

    // [이펙트 연출]
    public string EffectSpriteName; // 이펙트 이미지
    public float EffectSpriteScale; // 이펙트 크기
    public Vector3 EffectSpritePos;  // 이펙트 XY값
    public float ScenarioEffectVolume; // 이펙트소리

    // [BGM]
    public string bgmName; // ID범위에 흐르는브금 

    // [캐릭터 설정 - 2명 모두 저장]
    public Vector3 Char1Pos;
    public float Char1Scale;
    public float Char1Rotation;

    public Vector3 Char2Pos; 
    public float Char2Scale;
    public float Char2Rotation;

    // [이미지]
    public string BackgroundSpriteName; // 뒷배경
    public string IllustName; // 1980 x 1080일러스트

    // [선택지]
    public int LastChoiceResult; // 선택지 값
}