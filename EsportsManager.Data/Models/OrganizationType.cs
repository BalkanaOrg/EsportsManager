using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportsManager.EsportsManager.Data.Models
{
    public enum OrganizationType
    {
        Team, // G2, TSM, Fnatic, etc
        TO, // ESL, DreamHack, Blast, PGL
        League, // LCS, LEC, LCK, OWL
        Publisher, // Valve, Riot, Blizzard, etc
        Media, // Last Free Nation / Sheep Esports / 
        Union, //Player/Team/League Union (LCSPA)
    }
}
