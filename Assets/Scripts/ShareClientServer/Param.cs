using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Param<T>
{
    public string codeName;
    public List<T> stats;
}

[System.Serializable]
public class AParam<T>
{
    public string Tkey;
    public T Tvalue;
}

[System.Serializable]
public class AParam<T1, T2>
{
    public T1 Tkey;
    public T2 Tvalue;
}

[System.Serializable]
public class ParamsForEachChapter<T>
{
    public string codename;
    public List<AParam<T>> _params;

    public void AddParam(AParam<T> param)
    {
        _params.Add(param);
    }
    public void Clone(ParamsForEachChapter<T> a)
    {
        this.codename = a.codename;
        this._params = new List<AParam<T>>();
        for (int i = 0; i < a._params.Count; i++)
        {
            AParam<T> tmp = new AParam<T>();
            tmp.Tkey = a._params[i].Tkey;
            tmp.Tvalue = a._params[i].Tvalue;
            this._params.Add(tmp);
        }
    }
}