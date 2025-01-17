﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp_Oblig2.Model;
using System.Diagnostics.CodeAnalysis;
using ReactApplication.Models;
using WebApp_Oblig2.DAL;

namespace ReactApplication.DAL
{
    [ExcludeFromCodeCoverage]
    public class ReiseRepository : IReiseRepository
    {
        // Oppretter objekt av typen DB
        private readonly DB _db;

        // Konstruktør
        // Tildeler databasen til _db
        public ReiseRepository(DB db)
        {
            _db = db;
        }

        // Opprett ny rute
        // TODO: Skill mellom catch-false og try-false
        public async Task<Boolean> NyRute(RuteMod rute)
        {
            try
            {
                Rute eksisterer = await _db.Ruter.FirstOrDefaultAsync(r =>
                r.ruteFra == rute.ruteFra && r.ruteTil == rute.ruteTil);

                if (eksisterer == null)
                {
                    Rute nyRute = new Rute
                    {
                        ruteFra = rute.ruteFra,
                        ruteTil = rute.ruteTil,
                        dagsreise = rute.dagsreise,
                    };

                    await _db.Ruter.AddAsync(nyRute);
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Slett eksisterende rute
        public async Task<Boolean> SlettRute(int ruteId)
        {
            try
            {
                Rute eksisterer = await _db.Ruter.FirstOrDefaultAsync(r =>
                r.ruteId == ruteId);

                if (eksisterer != null)
                {
                    _db.Ruter.Remove(eksisterer);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Hent en rute
        public async Task<RuteMod> EnRute(int ruteId)
        {
            try
            {
                Rute funnetRute = await _db.Ruter.FirstOrDefaultAsync(r =>
                r.ruteId == ruteId);

                if (funnetRute != null)
                {
                    RuteMod temp_rute = new RuteMod
                    {
                        id = funnetRute.ruteId,
                        ruteFra = funnetRute.ruteFra,
                        ruteTil = funnetRute.ruteTil,
                        dagsreise = funnetRute.dagsreise
                    };

                    return temp_rute;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // Hent alle ruter
        public async Task<List<RuteMod>> AlleRuter()
        {
            try
            {
                List<Rute> ruter = await _db.Ruter.ToListAsync();
                List<RuteMod> returnRuter = new List<RuteMod>();

                foreach (var rute in ruter)
                {
                    RuteMod temp_rute = new RuteMod
                    {
                        id = rute.ruteId,
                        ruteFra = rute.ruteFra,
                        ruteTil = rute.ruteTil,
                        dagsreise = rute.dagsreise
                    };

                    returnRuter.Add(temp_rute);
                }

                return returnRuter;
            }
            catch
            {
                return null;
            }
        }

        // Oppdater en rute
        public async Task<Boolean> oppdaterRute(RuteMod rute)
        {
            try
            {
                Rute funnetRute = await _db.Ruter.FirstOrDefaultAsync(r =>
                    r.ruteId == rute.id);

                if (funnetRute != null)
                {
                    funnetRute.dagsreise = rute.dagsreise;
                    funnetRute.ruteFra = rute.ruteFra;
                    funnetRute.ruteTil = rute.ruteTil;

                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Opprett ny reise
        // Sjekker om reise eksisterer ved bruk av avreisedato
        public async Task<Boolean> NyReise(ReiseMod reise)
        {
            try
            {
                Reise funnetReise =
                    await _db.Reiser.FirstOrDefaultAsync(
                        r => r.ReiseDatoTid == reise.reiseDatoTid &&
                        r.RuteId.ruteFra == reise.ruteFra &&
                        r.RuteId.ruteTil == reise.ruteTil);

                Rute funnetRute = await _db.Ruter.FirstOrDefaultAsync(r =>
                r.ruteFra == reise.ruteFra && r.ruteTil == reise.ruteTil);

                if (funnetReise == null)
                {

                    Reise nyReise = new Reise
                    {
                        ReiseDatoTid = reise.reiseDatoTid,
                        RuteId = funnetRute,
                        PrisBarn = reise.prisBarn,
                        PrisVoksen = reise.prisVoksen,
                        PrisLugarStandard = reise.prisLugarStandard,
                        PrisLugarPremium = reise.prisLugarPremium,
                    };

                    await _db.Reiser.AddAsync(nyReise);
                    await _db.SaveChangesAsync();
                    return true;

                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Slett en eksisterende reise
        public async Task<Boolean> SlettReise(int reiseId)
        {
            try
            {
                Reise funnetReise = await _db.Reiser.FirstOrDefaultAsync(r =>
                r.ReiseId == reiseId);

                if (funnetReise != null)
                {
                    _db.Remove(funnetReise);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Hent en eksisterende reise
        public async Task<ReiseMod> EnReise(int reiseId)
        {
            try
            {
                Reise funnetReise = await _db.Reiser.FirstOrDefaultAsync(
                    r => r.ReiseId == reiseId);

                if (funnetReise != null)
                {
                    ReiseMod tempReise = new ReiseMod
                    {
                        reiseDatoTid = funnetReise.ReiseDatoTid,
                        ruteFra = funnetReise.RuteId.ruteFra,
                        ruteTil = funnetReise.RuteId.ruteTil,
                        dagsreise = funnetReise.RuteId.dagsreise,
                        prisBarn = funnetReise.PrisBarn,
                        prisVoksen = funnetReise.PrisVoksen,
                        prisLugarStandard = funnetReise.PrisLugarStandard,
                        prisLugarPremium = funnetReise.PrisLugarPremium
                    };

                    return tempReise;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Oppdater eksisterende reise
        public async Task<Boolean> OppdaterReise(ReiseMod reise)
        {
            try
            {
                Reise funnetReise = await _db.Reiser.FirstOrDefaultAsync(r =>
                r.ReiseId == reise.id);

                if (funnetReise != null)
                {
                    funnetReise.ReiseDatoTid = reise.reiseDatoTid;
                    funnetReise.PrisBarn = reise.prisBarn;
                    funnetReise.PrisVoksen = reise.prisVoksen;
                    funnetReise.PrisLugarStandard = reise.prisLugarStandard;
                    funnetReise.PrisLugarPremium = reise.prisLugarPremium;

                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Hent alle eksisterende reiser
        public async Task<List<ReiseMod>> AlleReiser(int id)
        {
            try
            {
                List<Reise> reiserDB = await _db.Reiser.ToListAsync();
                List<ReiseMod> reiser = new List<ReiseMod>();

                foreach (var reise in reiserDB)
                {
                    if (reise.RuteId.ruteId == id)
                    {
                        ReiseMod reiseObjekt = new ReiseMod
                        {
                            id = reise.ReiseId,
                            reiseDatoTid = reise.ReiseDatoTid,
                            ruteFra = reise.RuteId.ruteFra,
                            ruteTil = reise.RuteId.ruteTil,
                            prisBarn = reise.PrisBarn,
                            prisVoksen = reise.PrisVoksen,
                            prisLugarStandard = reise.PrisLugarStandard,
                            prisLugarPremium = reise.PrisLugarPremium,
                            dagsreise = reise.RuteId.dagsreise
                        };

                        reiser.Add(reiseObjekt);
                    }
                }

                if (reiser != null)
                {
                    return reiser;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
    }
}