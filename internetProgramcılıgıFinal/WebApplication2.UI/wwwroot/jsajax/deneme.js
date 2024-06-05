// $(document).ready(function () {
//     listele();
// });
// var baseurl = "https://localhost:7110/";
// var gettoken = localStorage.getItem("token");
// function listele() {
//     $.ajax({
//         type: "GET",
//         url: baseurl + "api/Houses/GetDetailList",
//         contentType: "application/json",
//         headers: {
//             "Authorization": "Bearer " + gettoken
//         },
//         success: function (response) {
//             console.log(response);
//             var html = "";
//             $.each(response, function (index, item) {
//                 html += `
//                 <div class="col-md-4">
//                     <div class="card mb-4 shadow-sm">
//                         <div class="slick-slider">
//                             <img src="${item.houseImages.image1}" class="card-img-top" alt="${item.name}">

//                         </div>
//                         <div class="card-body">
//                             <h5 class="card-title">${item.name}</h5>
//                             <p class="card-text">Oda Sayısı${item.roomCount}</p>
//                             <p class="card-text">Kat Sayısı :${item.floorCount}</p>
//                             <p class="card-text">Fiyat: ${item.price}₺</p>
//                             <p class="card-text">İl:${item.district.name}</p>
//                             <p class="card-text">İlçe :${item.province.name}</p>
//                             <p class="card-text">Kullanici Adi: ${item.appUser.userName}</p>
//                         </div>
//                     </div>
//                 </div>`;
//             });
//             $("#urunListesi").html(html);

//             // Slick slider'ı etkinleştirme
//             $('.slick-slider').slick({
//                 slidesToShow: 1,
//                 slidesToScroll: 1,
//                 autoplay: true,
//                 autoplaySpeed: 2000,
//                 dots: true,
//                 arrows: false
//             });
//         },
//         error: function (xhr, status, error) {
//             console.error("Error: " + status + " " + error);
//         }
//     });
// }