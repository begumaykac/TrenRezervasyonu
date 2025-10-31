using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public class RezervasyonService : IRezervasyonService
    {
       public RezervasyonRes RezervasyonYap(RezervasyonReq request)
        {
            var response = new RezervasyonRes
            {
                RezervasyonYapilabilir = false,
                YerlesimAyrinti = new List<YerlesimDetay>()
            };

            //// Kullanılabilir vagonlar
            var kullanilabilirVagonlar = request.Tren.Vagonlar
                .Where(v => v.Kapasite > 0 && v.DoluKoltukAdet < v.Kapasite * 0.7) // %70 doluluk sınırı
                .Select(v => new
                {
                    Vagon = v,
                    BosKoltuk = (int)(v.Kapasite * 0.7) - v.DoluKoltukAdet
                })
                .Where(x => x.BosKoltuk > 0)
                .OrderByDescending(x => x.BosKoltuk)
                .ToList();

            if (!kullanilabilirVagonlar.Any()) // uygun vagon yoksa rezervasyon yapılamaz
                return response;

            var kisiSayisi = request.RezervasyonYapilacakKisiSayisi;

            // Tek vagon yerleştirme
            var tekVagon = kullanilabilirVagonlar.FirstOrDefault(x => x.BosKoltuk >= kisiSayisi);
            
            if (tekVagon != null)
            {
                response.RezervasyonYapilabilir = true;
                response.YerlesimAyrinti.Add(new YerlesimDetay
                {
                    VagonAdi = tekVagon.Vagon.Ad,
                    KisiSayisi = kisiSayisi /// tüm kişiler tek vagona yerleştirilir
                });
                return response;
            }

            // Farklı vagonlara yerleştirme
            if (request.KisilerFarkliVagonlaraYerlestirilebilir)
            {
                var toplamBosKoltuk = kullanilabilirVagonlar.Sum(x => x.BosKoltuk);
                
                if (toplamBosKoltuk >= kisiSayisi)
                {
                    response.RezervasyonYapilabilir = true;
                    var kalanKisi = kisiSayisi;

                    foreach (var vagonBilgi in kullanilabilirVagonlar)
                    {
                        if (kalanKisi <= 0) break; 

                        var yerlestirilecek = Math.Min(kalanKisi, vagonBilgi.BosKoltuk);
                        
                        response.YerlesimAyrinti.Add(new YerlesimDetay
                        {
                            VagonAdi = vagonBilgi.Vagon.Ad,
                            KisiSayisi = yerlestirilecek
                        });

                        kalanKisi -= yerlestirilecek; //güncellenen kalan kişi sayısı
                    }
                }
            }

            return response;
        }

    }
    
}