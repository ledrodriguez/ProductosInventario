document.getElementById('product-in-form').addEventListener('submit', async (e) => {
    e.preventDefault();
    const productId = document.getElementById('product-id-in').value;
    const quantity = document.getElementById('product-quantity-in').value;
    
    const response = await fetch('http://localhost:5001/api/products/agregarproducto', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': localStorage.getItem('token')
        },
        body: JSON.stringify({ productId, quantity })
    });
    
    const result = await response.json();
    if (response.ok) {
        alert('Registro del producto ingresado satisfactoriamente');
    } else {
        alert('Error: ' + result.message);
    }
});

document.getElementById('product-out-form').addEventListener('submit', async (e) => {
    e.preventDefault();
    const productId = document.getElementById('product-id-out').value;
    const quantity = document.getElementById('product-quantity-out').value;
    
    const response = await fetch('http://localhost:5001/products/out', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ productId, quantity })
    });
    
    const result = await response.json();
    if (response.ok) {
        alert('Salida del producto registrada satisfactoriamente');
    } else {
        alert('Error: ' + result.message);
    }
});

document.getElementById('product-check-form').addEventListener('submit', async (e) => {
    e.preventDefault();
    const productId = document.getElementById('product-id-check').value;
    
    const response = await fetch(`http://localhost:5001/products/${productId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });
    
    const result = await response.json();
    const resultDiv = document.getElementById('result');
    if (response.ok) {
        resultDiv.innerText = `Producto encontrado: ID = ${result.id}, Nombre = ${result.name}, Cantidad = ${result.quantity}`;
    } else {
        resultDiv.innerText = 'Producto no encontrado';
    }
});