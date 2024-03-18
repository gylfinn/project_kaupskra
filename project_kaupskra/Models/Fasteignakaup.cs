namespace project_kaupskra.Models
{
public class Fasteignakaup
{
    public int ID { get; set; }
    public int FAERSLUNUMER { get; set; }
    public int EMNR { get; set; }
    public string? SKJALANUMER { get; set; }
    public int FASTNUM { get; set; }
    public string? HEIMILISFANG { get; set; }
    public int POSTNR { get; set; }
    public int HEINUM { get; set; }
    public int SVFN { get; set; }
    public string? SVEITARFELAG { get; set; }
    public DateTime UTGDAG { get; set; }
    public DateTime THINGLYSTDAGS { get; set; }
    public int KAUPVERD { get; set; }
    public int FASTEIGNAMAT { get; set; }
    public string? FASTEIGNAMAT_GILDANDI { get; set; }
    public string? BRUNABOTAMAT_GILDANDI { get; set; }
    public string? BYGGAR { get; set; }
    public string? FEPILOG { get; set; }
    public decimal EINFLM { get; set; }
    public string? LOD_FLM { get; set; }
    public string? LOD_FLMEIN { get; set; }
    public string? FJHERB { get; set; }
    public string? TEGUND { get; set; }
    public int FULLBUID { get; set; }
    public int ONOTHAEFUR_SAMNINGUR { get; set; }
}
}
