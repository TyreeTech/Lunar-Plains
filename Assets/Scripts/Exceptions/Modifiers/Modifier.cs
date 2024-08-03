using System.Collections;
using System.Collections.Generic;


public abstract class Modifier
{
    public readonly int sortOrder;

    public Modifier (int sortOrder)
    {
        this.sortOrder = sortOrder;
    }
}
