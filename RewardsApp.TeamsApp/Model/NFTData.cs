using System.Collections.Generic;
using System;

namespace RewardsApp.TeamsApp.Model
{
    public class NFTData
    {
        public List<OwnedNft> ownedNfts { get; set; }
        public int totalCount { get; set; }
    }
    public class Attribute
    {
        public string value { get; set; }
        public string trait_type { get; set; }
    }
    public class Contract
    {
        public string address { get; set; }
    }
    public class Medium
    {
        public string raw { get; set; }
        public string gateway { get; set; }
    }
    public class OwnedNft
    {
        public Contract contract { get; set; }
        public string tokenId { get; set; }
        public string tokenType { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime timeLastUpdated { get; set; }
        public RawMetadata rawMetadata { get; set; }
        public TokenUri tokenUri { get; set; }
        public List<Medium> media { get; set; }
        public int balance { get; set; }
    }
    public class RawMetadata
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public List<Attribute> attributes { get; set; }
    }
    public class TokenUri
    {
        public string raw { get; set; }
        public string gateway { get; set; }
    }
}