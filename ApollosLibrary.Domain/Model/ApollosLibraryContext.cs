using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class ApollosLibraryContext : DbContext
    {

        public ApollosLibraryContext(DbContextOptions<ApollosLibraryContext> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ErrorCode> ErrorCodes { get; set; }
        public DbSet<FictionType> FictionTypes { get; set; }
        public DbSet<FormType> FormTypes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PublicationFormat> PublicationFormats { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<LibraryEntry> LibraryEntries { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FictionType>().HasData(new List<FictionType>()
            {
                new FictionType()
                {
                    TypeId = 1,
                    Name = "Non-Fiction",
                },
                new FictionType()
                {
                    TypeId = 2,
                    Name = "Fiction",
                }
            });

            modelBuilder.Entity<BusinessType>().HasData(new List<BusinessType>()
            {
                new BusinessType()
                {
                    BusinessTypeId = 1,
                    Name = "Publisher",
                },
                new BusinessType()
                {
                    BusinessTypeId = 2,
                    Name = "Bookshop",
                }
            });

            modelBuilder.Entity<FormType>().HasData(new List<FormType>()
            {
                new FormType()
                {
                    TypeId = 1,
                    Name = "Novel",
                },
                new FormType()
                {
                    TypeId = 2,
                    Name = "Novella",
                },
                new FormType()
                {
                    TypeId = 3,
                    Name = "Screenplay",
                },
                new FormType()
                {
                    TypeId = 4,
                    Name = "Manuscript",
                },
                new FormType()
                {
                    TypeId = 5,
                    Name = "Poem",
                },
                new FormType()
                {
                    TypeId = 6,
                    Name = "Textbook",
                },
            });

            modelBuilder.Entity<PublicationFormat>().HasData(new List<PublicationFormat>()
            {
                new PublicationFormat()
                {
                    TypeId = 1,
                    Name = "Printed",
                },
                new PublicationFormat()
                {
                    TypeId = 2,
                    Name = "eBook",
                },
                new PublicationFormat()
                {
                    TypeId = 3,
                    Name = "Audio Book",
                },
            });

            string countryString = "[{\"Name\": \"Afghanistan\",\"CountryId\": \"AF\"},{\"Name\": \"Åland Islands\",\"CountryId\": \"AX\"},{\"Name\": \"Albania\",\"CountryId\": \"AL\"}" +
                ",{\"Name\": \"Algeria\",\"CountryId\": \"DZ\"},{\"Name\": \"American Samoa\",\"CountryId\": \"AS\"},{\"Name\": \"Andorra\",\"CountryId\": \"AD\"}" +
                ",{\"Name\": \"Angola\",\"CountryId\": \"AO\"},{\"Name\": \"Anguilla\",\"CountryId\": \"AI\"},{\"Name\": \"Antarctica\",\"CountryId\": \"AQ\"}" +
                ",{\"Name\": \"Antigua and Barbuda\",\"CountryId\": \"AG\"},{\"Name\": \"Argentina\",\"CountryId\": \"AR\"},{\"Name\": \"Armenia\",\"CountryId\": \"AM\"}" +
                ",{\"Name\": \"Aruba\",\"CountryId\": \"AW\"},{\"Name\": \"Australia\",\"CountryId\": \"AU\"},{\"Name\": \"Austria\",\"CountryId\": \"AT\"}" +
                ",{\"Name\": \"Azerbaijan\",\"CountryId\": \"AZ\"},{\"Name\": \"Bahamas\",\"CountryId\": \"BS\"},{\"Name\": \"Bahrain\",\"CountryId\": \"BH\"}" +
                ",{\"Name\": \"Bangladesh\",\"CountryId\": \"BD\"},{\"Name\": \"Barbados\",\"CountryId\": \"BB\"},{\"Name\": \"Belarus\",\"CountryId\": \"BY\"}" +
                ",{\"Name\": \"Belgium\",\"CountryId\": \"BE\"},{\"Name\": \"Belize\",\"CountryId\": \"BZ\"},{\"Name\": \"Benin\",\"CountryId\": \"BJ\"},{\"Name\": \"Bermuda\",\"CountryId\": \"BM\"}" +
                ",{\"Name\": \"Bhutan\",\"CountryId\": \"BT\"},{\"Name\": \"Bolivia, Plurinational State of\",\"CountryId\": \"BO\"},{\"Name\": \"Bonaire, Sint Eustatius and Saba\",\"CountryId\": \"BQ\"}" +
                ",{\"Name\": \"Bosnia and Herzegovina\",\"CountryId\": \"BA\"},{\"Name\": \"Botswana\",\"CountryId\": \"BW\"},{\"Name\": \"Bouvet Island\",\"CountryId\": \"BV\"}" +
                ",{\"Name\": \"Brazil\",\"CountryId\": \"BR\"},{\"Name\": \"British Indian Ocean Territory\",\"CountryId\": \"IO\"},{\"Name\": \"Brunei Darussalam\",\"CountryId\": \"BN\"}" +
                ",{\"Name\": \"Bulgaria\",\"CountryId\": \"BG\"},{\"Name\": \"Burkina Faso\",\"CountryId\": \"BF\"},{\"Name\": \"Burundi\",\"CountryId\": \"BI\"}" +
                ",{\"Name\": \"Cambodia\",\"CountryId\": \"KH\"},{\"Name\": \"Cameroon\",\"CountryId\": \"CM\"},{\"Name\": \"Canada\",\"CountryId\": \"CA\"}" +
                ",{\"Name\": \"Cape Verde\",\"CountryId\": \"CV\"},{\"Name\": \"Cayman Islands\",\"CountryId\": \"KY\"},{\"Name\": \"Central African Republic\",\"CountryId\": \"CF\"}" +
                ",{\"Name\": \"Chad\",\"CountryId\": \"TD\"},{\"Name\": \"Chile\",\"CountryId\": \"CL\"},{\"Name\": \"China\",\"CountryId\": \"CN\"},{\"Name\": \"Christmas Island\",\"CountryId\": \"CX\"}" +
                ",{\"Name\": \"Cocos (Keeling) Islands\",\"CountryId\": \"CC\"},{\"Name\": \"Colombia\",\"CountryId\": \"CO\"},{\"Name\": \"Comoros\",\"CountryId\": \"KM\"}" +
                ",{\"Name\": \"Congo\",\"CountryId\": \"CG\"},{\"Name\": \"Congo, the Democratic Republic of the\",\"CountryId\": \"CD\"},{\"Name\": \"Cook Islands\",\"CountryId\": \"CK\"}" +
                ",{\"Name\": \"Costa Rica\",\"CountryId\": \"CR\"},{\"Name\": \"CÃ´te d'Ivoire\",\"CountryId\": \"CI\"},{\"Name\": \"Croatia\",\"CountryId\": \"HR\"}" +
                ",{\"Name\": \"Cuba\",\"CountryId\": \"CU\"},{\"Name\": \"CuraÃ§ao\",\"CountryId\": \"CW\"},{\"Name\": \"Cyprus\",\"CountryId\": \"CY\"},{\"Name\": \"Czech Republic\",\"CountryId\": \"CZ\"}" +
                ",{\"Name\": \"Denmark\",\"CountryId\": \"DK\"},{\"Name\": \"Djibouti\",\"CountryId\": \"DJ\"},{\"Name\": \"Dominica\",\"CountryId\": \"DM\"}," +
                "{\"Name\": \"Dominican Republic\",\"CountryId\": \"DO\"},{\"Name\": \"Ecuador\",\"CountryId\": \"EC\"},{\"Name\": \"Egypt\",\"CountryId\": \"EG\"}" +
                ",{\"Name\": \"El Salvador\",\"CountryId\": \"SV\"},{\"Name\": \"Equatorial Guinea\",\"CountryId\": \"GQ\"},{\"Name\": \"Eritrea\",\"CountryId\": \"ER\"}" +
                ",{\"Name\": \"Estonia\",\"CountryId\": \"EE\"},{\"Name\": \"Ethiopia\",\"CountryId\": \"ET\"},{\"Name\": \"Falkland Islands (Malvinas)\",\"CountryId\": \"FK\"}" +
                ",{\"Name\": \"Faroe Islands\",\"CountryId\": \"FO\"},{\"Name\": \"Fiji\",\"CountryId\": \"FJ\"},{\"Name\": \"Finland\",\"CountryId\": \"FI\"},{\"Name\": \"France\",\"CountryId\": \"FR\"}" +
                ",{\"Name\": \"French Guiana\",\"CountryId\": \"GF\"},{\"Name\": \"French Polynesia\",\"CountryId\": \"PF\"},{\"Name\": \"French Southern Territories\",\"CountryId\": \"TF\"}" +
                ",{\"Name\": \"Gabon\",\"CountryId\": \"GA\"},{\"Name\": \"Gambia\",\"CountryId\": \"GM\"},{\"Name\": \"Georgia\",\"CountryId\": \"GE\"},{\"Name\": \"Germany\",\"CountryId\": \"DE\"}" +
                ",{\"Name\": \"Ghana\",\"CountryId\": \"GH\"},{\"Name\": \"Gibraltar\",\"CountryId\": \"GI\"},{\"Name\": \"Greece\",\"CountryId\": \"GR\"},{\"Name\": \"Greenland\",\"CountryId\": \"GL\"}" +
                ",{\"Name\": \"Grenada\",\"CountryId\": \"GD\"},{\"Name\": \"Guadeloupe\",\"CountryId\": \"GP\"},{\"Name\": \"Guam\",\"CountryId\": \"GU\"},{\"Name\": \"Guatemala\",\"CountryId\": \"GT\"}" +
                ",{\"Name\": \"Guernsey\",\"CountryId\": \"GG\"},{\"Name\": \"Guinea\",\"CountryId\": \"GN\"},{\"Name\": \"Guinea-Bissau\",\"CountryId\": \"GW\"},{\"Name\": \"Guyana\",\"CountryId\": \"GY\"}" +
                ",{\"Name\": \"Haiti\",\"CountryId\": \"HT\"},{\"Name\": \"Heard Island and McDonald Islands\",\"CountryId\": \"HM\"},{\"Name\": \"Holy See (Vatican City State)\",\"CountryId\": \"VA\"}" +
                ",{\"Name\": \"Honduras\",\"CountryId\": \"HN\"},{\"Name\": \"Hong Kong\",\"CountryId\": \"HK\"},{\"Name\": \"Hungary\",\"CountryId\": \"HU\"},{\"Name\": \"Iceland\",\"CountryId\": \"IS\"}" +
                ",{\"Name\": \"India\",\"CountryId\": \"IN\"},{\"Name\": \"Indonesia\",\"CountryId\": \"ID\"},{\"Name\": \"Iran, Islamic Republic of\",\"CountryId\": \"IR\"}" +
                ",{\"Name\": \"Iraq\",\"CountryId\": \"IQ\"},{\"Name\": \"Ireland\",\"CountryId\": \"IE\"},{\"Name\": \"Isle of Man\",\"CountryId\": \"IM\"},{\"Name\": \"Israel\",\"CountryId\": \"IL\"}" +
                ",{\"Name\": \"Italy\",\"CountryId\": \"IT\"},{\"Name\": \"Jamaica\",\"CountryId\": \"JM\"},{\"Name\": \"Japan\",\"CountryId\": \"JP\"},{\"Name\": \"Jersey\",\"CountryId\": \"JE\"}" +
                ",{\"Name\": \"Jordan\",\"CountryId\": \"JO\"},{\"Name\": \"Kazakhstan\",\"CountryId\": \"KZ\"},{\"Name\": \"Kenya\",\"CountryId\": \"KE\"},{\"Name\": \"Kiribati\",\"CountryId\": \"KI\"}" +
                ",{\"Name\": \"Korea, Democratic People's Republic of\",\"CountryId\": \"KP\"},{\"Name\": \"Korea, Republic of\",\"CountryId\": \"KR\"},{\"Name\": \"Kuwait\",\"CountryId\": \"KW\"}" +
                ",{\"Name\": \"Kyrgyzstan\",\"CountryId\": \"KG\"},{\"Name\": \"Lao People's Democratic Republic\",\"CountryId\": \"LA\"},{\"Name\": \"Latvia\",\"CountryId\": \"LV\"}" +
                ",{\"Name\": \"Lebanon\",\"CountryId\": \"LB\"},{\"Name\": \"Lesotho\",\"CountryId\": \"LS\"},{\"Name\": \"Liberia\",\"CountryId\": \"LR\"},{\"Name\": \"Libya\",\"CountryId\": \"LY\"}" +
                ",{\"Name\": \"Liechtenstein\",\"CountryId\": \"LI\"},{\"Name\": \"Lithuania\",\"CountryId\": \"LT\"},{\"Name\": \"Luxembourg\",\"CountryId\": \"LU\"}" +
                ",{\"Name\": \"Macao\",\"CountryId\": \"MO\"},{\"Name\": \"Macedonia, the Former Yugoslav Republic of\",\"CountryId\": \"MK\"},{\"Name\": \"Madagascar\",\"CountryId\": \"MG\"}" +
                ",{\"Name\": \"Malawi\",\"CountryId\": \"MW\"},{\"Name\": \"Malaysia\",\"CountryId\": \"MY\"},{\"Name\": \"Maldives\",\"CountryId\": \"MV\"},{\"Name\": \"Mali\",\"CountryId\": \"ML\"}" +
                ",{\"Name\": \"Malta\",\"CountryId\": \"MT\"},{\"Name\": \"Marshall Islands\",\"CountryId\": \"MH\"},{\"Name\": \"Martinique\",\"CountryId\": \"MQ\"}" +
                ",{\"Name\": \"Mauritania\",\"CountryId\": \"MR\"},{\"Name\": \"Mauritius\",\"CountryId\": \"MU\"},{\"Name\": \"Mayotte\",\"CountryId\": \"YT\"}" +
                ",{\"Name\": \"Mexico\",\"CountryId\": \"MX\"},{\"Name\": \"Micronesia, Federated States of\",\"CountryId\": \"FM\"},{\"Name\": \"Moldova, Republic of\",\"CountryId\": \"MD\"}" +
                ",{\"Name\": \"Monaco\",\"CountryId\": \"MC\"},{\"Name\": \"Mongolia\",\"CountryId\": \"MN\"},{\"Name\": \"Montenegro\",\"CountryId\": \"ME\"}" +
                ",{\"Name\": \"Montserrat\",\"CountryId\": \"MS\"},{\"Name\": \"Morocco\",\"CountryId\": \"MA\"},{\"Name\": \"Mozambique\",\"CountryId\": \"MZ\"}" +
                ",{\"Name\": \"Myanmar\",\"CountryId\": \"MM\"},{\"Name\": \"Namibia\",\"CountryId\": \"NA\"},{\"Name\": \"Nauru\",\"CountryId\": \"NR\"},{\"Name\": \"Nepal\",\"CountryId\": \"NP\"}" +
                ",{\"Name\": \"Netherlands\",\"CountryId\": \"NL\"},{\"Name\": \"New Caledonia\",\"CountryId\": \"NC\"},{\"Name\": \"New Zealand\",\"CountryId\": \"NZ\"}" +
                ",{\"Name\": \"Nicaragua\",\"CountryId\": \"NI\"},{\"Name\": \"Niger\",\"CountryId\": \"NE\"},{\"Name\": \"Nigeria\",\"CountryId\": \"NG\"},{\"Name\": \"Niue\",\"CountryId\": \"NU\"}" +
                ",{\"Name\": \"Norfolk Island\",\"CountryId\": \"NF\"},{\"Name\": \"Northern Mariana Islands\",\"CountryId\": \"MP\"},{\"Name\": \"Norway\",\"CountryId\": \"NO\"}" +
                ",{\"Name\": \"Oman\",\"CountryId\": \"OM\"},{\"Name\": \"Pakistan\",\"CountryId\": \"PK\"},{\"Name\": \"Palau\",\"CountryId\": \"PW\"}" +
                ",{\"Name\": \"Palestine, State of\",\"CountryId\": \"PS\"},{\"Name\": \"Panama\",\"CountryId\": \"PA\"},{\"Name\": \"Papua New Guinea\",\"CountryId\": \"PG\"}" +
                ",{\"Name\": \"Paraguay\",\"CountryId\": \"PY\"},{\"Name\": \"Peru\",\"CountryId\": \"PE\"},{\"Name\": \"Philippines\",\"CountryId\": \"PH\"},{\"Name\": \"Pitcairn\",\"CountryId\": \"PN\"}" +
                ",{\"Name\": \"Poland\",\"CountryId\": \"PL\"},{\"Name\": \"Portugal\",\"CountryId\": \"PT\"},{\"Name\": \"Puerto Rico\",\"CountryId\": \"PR\"},{\"Name\": \"Qatar\",\"CountryId\": \"QA\"}" +
                ",{\"Name\": \"RÃ©union\",\"CountryId\": \"RE\"},{\"Name\": \"Romania\",\"CountryId\": \"RO\"},{\"Name\": \"Russian Federation\",\"CountryId\": \"RU\"}" +
                ",{\"Name\": \"Rwanda\",\"CountryId\": \"RW\"},{\"Name\": \"Saint BarthÃ©lemy\",\"CountryId\": \"BL\"},{\"Name\": \"Saint Helena, Ascension and Tristan da Cunha\",\"CountryId\": \"SH\"}" +
                ",{\"Name\": \"Saint Kitts and Nevis\",\"CountryId\": \"KN\"},{\"Name\": \"Saint Lucia\",\"CountryId\": \"LC\"},{\"Name\": \"Saint Martin (French part)\",\"CountryId\": \"MF\"}" +
                ",{\"Name\": \"Saint Pierre and Miquelon\",\"CountryId\": \"PM\"},{\"Name\": \"Saint Vincent and the Grenadines\",\"CountryId\": \"VC\"},{\"Name\": \"Samoa\",\"CountryId\": \"WS\"}" +
                ",{\"Name\": \"San Marino\",\"CountryId\": \"SM\"},{\"Name\": \"Sao Tome and Principe\",\"CountryId\": \"ST\"},{\"Name\": \"Saudi Arabia\",\"CountryId\": \"SA\"}" +
                ",{\"Name\": \"Senegal\",\"CountryId\": \"SN\"},{\"Name\": \"Serbia\",\"CountryId\": \"RS\"},{\"Name\": \"Seychelles\",\"CountryId\": \"SC\"}" +
                ",{\"Name\": \"Sierra Leone\",\"CountryId\": \"SL\"},{\"Name\": \"Singapore\",\"CountryId\": \"SG\"},{\"Name\": \"Sint Maarten (Dutch part)\",\"CountryId\": \"SX\"}" +
                ",{\"Name\": \"Slovakia\",\"CountryId\": \"SK\"},{\"Name\": \"Slovenia\",\"CountryId\": \"SI\"},{\"Name\": \"Solomon Islands\",\"CountryId\": \"SB\"}," +
                "{\"Name\": \"Somalia\",\"CountryId\": \"SO\"},{\"Name\": \"South Africa\",\"CountryId\": \"ZA\"},{\"Name\": \"South Georgia and the South Sandwich Islands\",\"CountryId\": \"GS\"}" +
                ",{\"Name\": \"South Sudan\",\"CountryId\": \"SS\"},{\"Name\": \"Spain\",\"CountryId\": \"ES\"},{\"Name\": \"Sri Lanka\",\"CountryId\": \"LK\"},{\"Name\": \"Sudan\",\"CountryId\": \"SD\"}" +
                ",{\"Name\": \"Suriname\",\"CountryId\": \"SR\"},{\"Name\": \"Svalbard and Jan Mayen\",\"CountryId\": \"SJ\"},{\"Name\": \"Swaziland\",\"CountryId\": \"SZ\"}" +
                ",{\"Name\": \"Sweden\",\"CountryId\": \"SE\"},{\"Name\": \"Switzerland\",\"CountryId\": \"CH\"},{\"Name\": \"Syrian Arab Republic\",\"CountryId\": \"SY\"}" +
                ",{\"Name\": \"Taiwan, Province of China\",\"CountryId\": \"TW\"},{\"Name\": \"Tajikistan\",\"CountryId\": \"TJ\"},{\"Name\": \"Tanzania, United Republic of\",\"CountryId\": \"TZ\"}" +
                ",{\"Name\": \"Thailand\",\"CountryId\": \"TH\"},{\"Name\": \"Timor-Leste\",\"CountryId\": \"TL\"},{\"Name\": \"Togo\",\"CountryId\": \"TG\"},{\"Name\": \"Tokelau\",\"CountryId\": \"TK\"}" +
                ",{\"Name\": \"Tonga\",\"CountryId\": \"TO\"},{\"Name\": \"Trinidad and Tobago\",\"CountryId\": \"TT\"},{\"Name\": \"Tunisia\",\"CountryId\": \"TN\"}" +
                ",{\"Name\": \"Turkey\",\"CountryId\": \"TR\"},{\"Name\": \"Turkmenistan\",\"CountryId\": \"TM\"},{\"Name\": \"Turks and Caicos Islands\",\"CountryId\": \"TC\"}" +
                ",{\"Name\": \"Tuvalu\",\"CountryId\": \"TV\"},{\"Name\": \"Uganda\",\"CountryId\": \"UG\"},{\"Name\": \"Ukraine\",\"CountryId\": \"UA\"}" +
                ",{\"Name\": \"United Arab Emirates\",\"CountryId\": \"AE\"},{\"Name\": \"United Kingdom\",\"CountryId\": \"GB\"},{\"Name\": \"United States\",\"CountryId\": \"US\"}" +
                ",{\"Name\": \"United States Minor Outlying Islands\",\"CountryId\": \"UM\"},{\"Name\": \"Uruguay\",\"CountryId\": \"UY\"},{\"Name\": \"Uzbekistan\",\"CountryId\": \"UZ\"}" +
                ",{\"Name\": \"Vanuatu\",\"CountryId\": \"VU\"},{\"Name\": \"Venezuela, Bolivarian Republic of\",\"CountryId\": \"VE\"},{\"Name\": \"Viet Nam\",\"CountryId\": \"VN\"}" +
                ",{\"Name\": \"Virgin Islands, British\",\"CountryId\": \"VG\"},{\"Name\": \"Virgin Islands, U.S.\",\"CountryId\": \"VI\"},{\"Name\": \"Wallis and Futuna\",\"CountryId\": \"WF\"}" +
                ",{\"Name\": \"Western Sahara\",\"CountryId\": \"EH\"},{\"Name\": \"Yemen\",\"CountryId\": \"YE\"},{\"Name\": \"Zambia\",\"CountryId\": \"ZM\"}" +
                ",{\"Name\": \"Zimbabwe\",\"CountryId\": \"ZW\"}]";

            var countries = JsonConvert.DeserializeObject<List<Country>>(countryString);

            modelBuilder.Entity<Country>().HasData(countries);

            modelBuilder.Entity<SubscriptionType>().HasData(new List<SubscriptionType>()
            {
                new SubscriptionType()
                {
                    MonthlyRate = 0.00m,
                    SubscriptionName = "Signed Up",
                    SubscriptionTypeId = -1,
                    Purchasable = false,
                    IsAvailable = true,
                    MaxUsers = 1,
                },
                new SubscriptionType()
                {
                    MonthlyRate = 0.00m,
                    SubscriptionName = "Staff Member",
                    SubscriptionTypeId = 1,
                    Purchasable= false,
                    IsAvailable = true,
                    MaxUsers = 1,
                },
                new SubscriptionType()
                {
                    MonthlyRate = 10.00m,
                    StripePriceId = "price_1L3eu4HSN4IIrwiZsUfrItzs",
                    SubscriptionName = "Individual Subscription",
                    SubscriptionTypeId = 2,
                    Purchasable = true,
                    IsAvailable = true,
                    MaxUsers = 1,
                    Description = "This subscription is for individuals keeping track of their own library.",
                },
                new SubscriptionType()
                {
                    MonthlyRate = 30.00m,
                    StripePriceId = "price_1L3euyHSN4IIrwiZvJYhpH2T",
                    SubscriptionName = "Family Subscription",
                    SubscriptionTypeId = 3,
                    Purchasable = true,
                    IsAvailable = false,
                    MaxUsers = 5,
                    Description = "This subscription is for families keeping track of their own libraries. Each user will have their own library.",
                },
            });

            var subscriptionId = Guid.NewGuid();

            modelBuilder.Entity<Subscription>().HasData(new Subscription()
            {
                SubscriptionTypeId = 1,
                ExpiryDate = DateTime.Now.AddYears(80),
                JoinDate = DateTime.Now,
                SubscriptionId = subscriptionId,
            });
        }
    }
}
