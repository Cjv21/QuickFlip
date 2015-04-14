using System.Collections.Generic;

public enum CommunityAbbrev
{
    IU = 1,
    MSU = 2,
    NU = 3,
    OSU = 4,
    PSU = 5, 
    PU = 6,
    UI = 7,
    UIUC = 8, 
    UM = 9,
    UMN = 10,
    UNL = 11, 
    UWM = 12
}

public enum PostType
{
    Buy,
    Sell
}

public enum AuctionType
{
    Auction,
    FavoriteOffer
}

public enum TransactionType
{
    Local,
    LocalOrLongDistance
}

public enum Category
{
    Auto, 
    Books, 
    CameraPhoto, 
    CellPhones, 
    ClothingShoe, 
    Computers,
    Electronics,
    HealthBeauty, 
    Home, 
    Jobs, 
    Movies, 
    Music, 
    MusicalInstruments,
    Pets, 
    RealEstate, 
    SportingGoods
}

public enum AlertMode
{
    Email,
    Text,
    Both
}

public enum Carrier
{
    ATT,
    Verizon,
    TMobile,
    Sprint,
    VirginMobile,
    USCellular,
    Nextel,
    Boost,
    Alltell

   /* AT&T – cellnumber@txt.att.net
    * Verizon – cellnumber@vtext.com
    * T-Mobile – cellnumber@tmomail.net
    * Sprint PCS - cellnumber@messaging.sprintpcs.com
    * Virgin Mobile – cellnumber@vmobl.com
    * US Cellular – cellnumber@email.uscc.net
    * Nextel - cellnumber@messaging.nextel.com
    * Boost - cellnumber@myboostmobile.com
    * Alltel – cellnumber@message.alltel.com
    */
}


public enum AlertType
{
    Outbid,
    NewOffer,
    Accepted,
    Lost
}