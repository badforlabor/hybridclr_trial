using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using test_proto;
using UnityEngine;

public class ProtoTestMono
{
    public void Test()
    {
        var resMap = new ResResArray();
        var resData = new ResRes()
            {id_key = 1, comment = "123", high_path = "high_path", low_path = "low_path", middle_path = "middle_path"};
        resMap.items.Add(resData);
        resMap.teams.Add(TeamFlag.TeamA);
        resMap.map_int32_team.Add(1, TeamFlag.TeamB);
        var data = resMap.ToByteArray();

        var resMapClone = ResResArray.Parser.ParseFrom(data);
        MyDebug.AssertDetail(resMap.items.Count == 1);
        
        var item0 = resMap.items[0];
        MyDebug.AssertDetail(item0.id_key == resData.id_key);
        MyDebug.AssertDetail(item0.comment == resData.comment);
        MyDebug.AssertDetail(item0.high_path == resData.high_path);
        MyDebug.AssertDetail(item0.low_path == resData.low_path);
        MyDebug.AssertDetail(item0.middle_path == resData.middle_path);
        
        MyDebug.AssertDetail(resMapClone.teams[0] == TeamFlag.TeamA);
        MyDebug.AssertDetail(resMapClone.map_int32_team[1] == TeamFlag.TeamB);
        
    }
}
