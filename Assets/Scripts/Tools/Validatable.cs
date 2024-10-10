using System.Collections.Generic;
using UnityEngine;

public abstract class Validatable : MonoBehaviour
{
    protected void ValidateObject(object obj, string objName)
    {
        if (obj == null)
            throw new System.Exception($"{objName} n�o foi atribu�do corretamente.");
    }

    protected void ValidateList<T>(IList<T> list, string listName)
    {
        if (list == null || list.Count == 0)
            throw new System.Exception($"A lista {listName} est� nula ou vazia.");
    }
}