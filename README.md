# Manyetik Kurye

Karmaşık Oyun Mekanikleri, Hafta 7 - Üç Mekanik, Dört Oyun

**Öğrenci:** Semih Yusuf Şentürk  
**Unity sürümü:** 6000.0.60f1  
**Platform:** Windows, 2D

Manyetik Kurye, metal bir küreyle kapıları kırıp tesisten çıkmaya çalışan bir robotun oyunudur.
Atış tuşunun basılı tutulma süresi kürenin hızını belirler. Küreyi taşımak hareketi
ağırlaştırır; küreyi atınca karakter hafifler. Kapıların kırılması bölüm ilerlemesini açar.

## Unity projesini açma

1. Depoyu indir veya klonla.
2. Unity Hub içinde **Add project from disk** ile bu klasörü seç.
3. **Project > Assets > Scenes** klasöründen bir sahne aç.
4. Play'e bas ve Game penceresine tıkla.
5. Sahne değiştirmek için Play'i durdur ve başka sahneyi çift tıkla. Oyun içinde sahne geçişi yoktur.

| Sahne | Kontroller | Amaç |
|---|---|---|
| `A_TekTus.unity` | Space'e kısa bas veya basılı tutup bırak | Hareketli aralıktan 6 atışı hedefe geçir |
| `B_HareketHissi.unity` | A/D veya sol/sağ: hareket; Space: zıpla; Tab: hafif/yüklü profil | Parkurun sonuna ulaş |
| `C_Carpisma.unity` | W/S veya yukarı/aşağı: açı; Space: atış gücü | 3 bloğu kır, sonra küreyi yeşil hedefe gönder |
| `D_Birlesik.unity` | A/D: hareket; Space: zıpla; W/S: açı; E: atış gücü; F: küreyi al/çağır | 3 kapıyı kır, küreyi elinde taşıyarak çıkışa ulaş |

**R:** aynı sahneyi yeniden başlatır. Space'i erken bırakmak zıplamayı kısaltır.
Atış tuşunu 1 saniye tutmak tam güç verir. Bir bloğun üstündeki **1 DARBE / 2 DARBE**
yazısı gereken yeterli vuruş sayısıdır; sekme sayısı değildir. C'deki eğimli plaka
sekme için kullanılabilir; doğrudan isabetler de hasar verir.

D'de F, 2 birim yakındaki küreyi ücretsiz alır; daha uzaktaki küreyi çağırmak 1 hak harcar.
Küre eldeyken ağır, küre atıldığında hafif hareket profili kullanılır. Uzun boşlukta küreyi
önce karşıya atıp hafif halde zıplamak gerekir. Çıkış, üç kapı kırılmadan ve küre elde olmadan tamamlanmaz.
D'de darbe eşiği geçen her saniye 0,01 artar; üst sınır 17'dir.

## Windows build

`Build/D_Birlesik/Manyetik Kurye.exe` dosyasını çalıştır. Bu build yalnızca D oyununu açar.
Yanındaki `Manyetik Kurye_Data`, `MonoBleedingEdge`, `D3D12` klasörleri ve DLL dosyaları
birlikte kalmalıdır. A, B ve C oyunları Unity'deki sahnelerden oynanır.

## Teslim dosyaları

```text
Assets/
  Scenes/
    A_TekTus.unity
    B_HareketHissi.unity
    C_Carpisma.unity
    D_Birlesik.unity
  Scripts/
    A/         MovingGate
    B/         CourierMovement
    C/         EnergyBall, BreakableBlock
    D/         MagnetLauncher, GameSession, GoalZone
    Ortak/     GameHud, FollowCamera
  Prefabs/     NormalBlock, ArmoredBlock
Packages/
ProjectSettings/
Build/D_Birlesik/
Belge/
  H7_TasarimBelgesi.pdf
  oynanis_D.mp4
README.md
```

Scriptler sahneler arasında ortak kullanılır. `.meta` dosyaları Unity nesne ve script
bağlantılarını korur. `Library`, `Temp`, `Logs` ve `UserSettings` depoya dahil edilmez.
Oynanış videosu yaklaşık 37 saniyedir. Tasarım belgesi dört sayfadır.

## Yapay zekâ beyanı

OpenAI Codex; ilk sahnelerin oluşturulması, hata düzeltmeleri,
HUD sadeleştirmesinde kullanıldı.
Etkilenen scriptler: `MovingGate`, `CourierMovement`, `EnergyBall`,
`MagnetLauncher`, `GameSession`.
Ayrıntılı kapsam tasarım belgesinde belirtilmiştir.
