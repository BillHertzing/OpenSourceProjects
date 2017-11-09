using System;
public interface IWebGetHttpResponseCharacteristics
{
    int Follow3xxDepth { get; set; }
    bool Follow3xx { get; set; }

    Type TypeName { get ; set; }
}


