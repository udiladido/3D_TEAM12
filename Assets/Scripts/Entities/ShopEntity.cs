
using System;
using System.Collections.Generic;

[Serializable]
public class ShopEntity : EntityBase
{
    
    public List<ShopSaleEntity> shopSaleEntities;
}


[Serializable]
public class ShopSaleEntity
{
    public int shopId;
    public int itemId;
    public long price;
}