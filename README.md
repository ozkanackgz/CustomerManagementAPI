Proje C# tabanlı ASP.NET Core Web API kullanılarak geliştirilmiş müşteri yönetim sistemi API' si ve yetkilendirme uygulamasıdır, 
C# ile Entity Framework Core kullanarak SQL Server bağlantısı sağlamak ve veri işlemlerini gerçekleştirmek üzere veri tabanı oluşturulmuştur.



Projenin Çalıştırılması :

1-Dosyayı zip olarak indirin


2-Visual Studio üzerinden projeyi açın


3-appsettings.json dosyası üzerinden veri tabanı bağlantınızı kontrol edin, kendi bilgisayarınıza uygun yapılandırmaları sağlayabilirsiniz


4-Microsoft.EntityFrameWorkCore ve Microsoft.EntityFrameWorkCore.SqlServer paketlerinin yüklü olduğundan emin olun


5-Microsoft.AspNetCore.AuthenticationJwtBearer ve System.IdentityModel.TokensJwt paketlerinin yüklü olduğundan emin olun


6-Programı Çalıştırın





Projenin Oluşturulma Aşamaları :

1-Visual Studio aracı ile ASP.NET Core Web API olarak proje şablonu belirlendi


2-Projenin adını (CustomerManagementAPI) ve konumunu belirledikten sonra proje yapılandırılması sağlandı

    --> Authentication Type (None) - Yetkilendirme adımını daha sonra belirlemek için

    --> Enable OpenAPI Support Kutucuğu İşaretli - Swagger dökümantasyonu için


3-Müşteri modelini oluşturmak amacıyla Solution Explorer da "Model" klasörü oluşturuldu ve "Customer.cs" sınıfı eklemek üzere müşteri özellikleri burada kodlandı


4-Veri tabanı bağlantısı amacıyla Solution Explorer da "Data" klasörü oluşturuldu ve "AppDBContext.cs" sınıfı eklendi ve gerekli kodlar yazıldı


5-SQL Server bağlantısı yapılandırmak amacıyla "appsettings.json" dosyasına bağlantı metni eklendi ve LocalDB kullanılarak SQL Server a erişildi


6-"AppDbContext" için bağlılık enjeksiyonu eklendi


7-Veri tabanı oluşturmak ve Customers tablosunu eklemek amacıyla Package Manager Console ile;

      Add-Migration InitialCreate
      Update-Database

  Komutları sırasıyla çalıştırıldı

  Bu aşamada console "ObjectNotFound" hatası döndürdü ve araştırma neticesinde Microsoft.EntityFrameWorkCore.Tools paketinin yüklü olmamasından kaynaklı olduğu anlaşıldı;

      Install-package microsoft.entityframeworkcore.tools 

  Komutu ile hata çözüldü ve gerekli yapılandırma sağlandı



8- -Model Class- olarak "Customer" ve -Data Context Class- olarak "AppDBContext" olmak üzere bir CustomerController oluşturuldu ve;
       
      Visual Studio, CRUD işlemleri için temel endpointler içeren bir sınıf oluşturdu


9-Proje ilk defa çalıştırıldı ve https://localhost:7226/swagger gizlilik hatası alınarak erişim sağlanamadı

  Gerekli araştırma neticesinde sorunun güvenlik sertifikasıyla ilgili olduğu tespit edildi ve komut istemicisinde;

      dotnet dex-certs https --trust

  Komutu çalıştırılarak geliştirme ortamında kullanılan güvenlik sertifikası güvenilir olarak işaretlendi ve erişim sağlandı



10-JWT Rol Bazlı Yetkilendirme ve Test aşamalarına geçmeden önce projenin genel değerlendirmesi yapıldı ve;

   GET api/Customers
   
   POST api/Customers
   
   GET api/Customers/{id}
   
   PUT api/Customers/{id}
   
   DELETE api/Customers/{id}
   

   EndPoint leri test edildi ve uygulmanın çalıştığı görüldü



11-JWT Rol Bazlı Yetkilendirme;

   --> aspsettings.json dosyasına gerekli JWT ayarı eklendi ve JWT imzasında kullanılacak güçlü bir anahtar seçildi

   --> Program.cs dosyasında JWT ayarları yapılandırıldı ve gerekli kodlar eklendi

   --> Token Oluşturma; AuthController adında bir Controller sınıfı oluşturuldu ve gerekli kodlar eklendi

   --> Models klasörüne gerekli yetkilendirmeleri sağlamak amacıyla "Role.cs", "User.cs" sınıfları eklendi ve gerekli kodlamalar yapıldı

   --> Rol bazlı yetkilendirme sağlamak amacıyla CustomerController sınıfına [Authorize], [Authorize(Roles = "Admin")], [Authorize(Roles = "User")]
       attributeleri eklendi ve gerekli kod yapılandırmaları sağlandı

   --> Admin CRUD işlemleri yapabilir
       User Get işlemi yapabilir



12-Test;

   --> Login işlemi yapılmadan hiçbir CRUD işleminin yapılamaması sağlandı ve test edildi (Başarılı)

   --> Admin veya User olarak token alma işlemleri sağlandı ve test edildi (Başarılı)

   --> Tokenler ile Authorize üzerinde giriş işlemleri yapıldı ve erişim yetkileri test edildi (Başarısız)



13-Problem;

   --> Alınan tokenler ile giriş yapıldığında "Error : Response Status İs 401" Yetkisizlik Hatası alınmakta ve çözülemedi.
