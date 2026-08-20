// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// const form = document.querySelector("form")

// form.addEventListener("submit", async function(event){
//     event.preventDefault();

//     const name = document.querySelector("#name").value;
//     const price = Number(document.querySelector("#price").value);
//     const image = document.querySelector("file").value;

//     json = JSON.stringify({name: name, price: price});

//     const response = await fetch("/product", {
//         method: "POST",
//         body: json,
//         headers: {
//             "Content-Type": "application/json"
//         }
//     });

//     const product = await response.json();

//     window.open(`/Home/Product/${product.id}`, "_self");
// })

var shoppingCart = [] 
localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart))

function updateCart(id) {
    var currentShoppingCart = JSON.parse(localStorage.getItem("shoppingCart"));
    const shoppingIcon = document.getElementById(`cart-icon-${id}`);

    if (shoppingIcon.classList.contains("bi-cart-plus")){
        shoppingIcon.classList.replace("bi-cart-plus", "bi-cart-check-fill")
        currentShoppingCart.push(id)
    } else {
        shoppingIcon.classList.replace("bi-cart-check-fill", "bi-cart-plus")
        currentShoppingCart.splice(currentShoppingCart.indexOf(id), 1)
    }

    localStorage.setItem("shoppingCart", JSON.stringify(currentShoppingCart))


}