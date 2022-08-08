[System.Serializable]
public class Option
{
    public string OptionId;
    public string NextEventId;
    public string Text;
    public string[] ItemRequired;
    public string[] SkillRequired;
}

[System.Serializable]
public class EventData
{
    public string EventId;
    public string Type;
    public string Name;
    public string ImgName;
    public string [] Script;
    public int OptionCount;
    public Option Option1;
    public Option Option2;
    public Option Option3;
    public Option Option4;

    public EventData()
    {
        Option1 = new OptionButton1();
        Option2 = new OptionButton2();
        Option3 = new OptionButton3();
        Option4 = new OptionButton4();
    }

    [System.Serializable]
    public class OptionButton1 : Option { }

    [System.Serializable]
    public class OptionButton2 : Option { }

    [System.Serializable]
    public class OptionButton3 : Option { }

    [System.Serializable]
    public class OptionButton4 : Option { }
}