using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CharacterData
{
    public int index;
    public CharacterType type;
}

public class CharacterManager : MX.BaseManager
{
    private static CharacterManager _instance = null;
    public static CharacterManager Instance
    {
        get { return _instance; }
    }

    private int _character_index = 0;
    [SerializeField]
    private List<Character> _character_list = new List<Character>();


    public override void OnInitialize()
    {
        CharacterManager._instance = this;
        base.OnInitialize();
    }

    protected override void OnReady()
    {
        base.OnReady();


        foreach(var character in this._character_list)
        {
            CharacterData data = new CharacterData()
            {
                index = ++this._character_index,
                type = CharacterType.Base
            };
            character.InitCharacter(data);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected virtual void OnUpdateCharactersMove()
    {
        foreach (var character in this._character_list)
        {
            if(character.has_controller) { continue; }

            this.OnUpdateCharacterMove(character, Vector3.zero);
        }
    }

    protected virtual void OnUpdateCharacterMove(Character character, Vector3 direction)
    {
        if (character == null)
        {
            return;
        }

        character.UpdateMovement(direction);

        //
    }

    // Update is called once per frame
    public override void OnUpdateFrame()
    {
        base.OnUpdateFrame();

        this.OnUpdateCharactersMove();
    }
}
