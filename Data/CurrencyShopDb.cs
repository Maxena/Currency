using CurrencyShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Data
{
    public class CurrencyShopDb: DbContext
    {
        public CurrencyShopDb(DbContextOptions<CurrencyShopDb> options) : base(options)
        {

        }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Prices> Prices { get; set; }
        public DbSet<PannelUser> PannelUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Objects> Objects { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<AddMobToken> AdMob { get; set; }
        public DbSet<Ads> Ads { get; set; }
        public DbSet<ChatRoom> chatRooms { get; set; }
        public DbSet<Words> words { get; set; }
        public DbSet<AuthenticateChatUser> authenticateChatUsers { get; set; }




    }
}
