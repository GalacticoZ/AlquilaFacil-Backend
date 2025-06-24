<script setup>
import { onMounted, ref } from 'vue';

const props = defineProps({
  price: Number
});

const loaded = ref(false);
const paidFor = ref(false);

const paypalButton = ref(null);

const setLoaded = () => {
  loaded.value = true;

  window.paypal
    .Buttons({
      createOrder: (data, actions) => {
        return actions.order.create({
          purchase_units: [
            {
              amount: {
                currency_code: 'USD',
                value: props.price.toString()
              }
            }
          ]
        });
      },
      onApprove: async (data, actions) => {
        const details = await actions.order.capture();
        paidFor.value = true;
        alert('Pago completado por ' + details.payer.name.given_name);
      },
      onError: (err) => {
        console.error('Error en el pago', err);
      }
    })
    .render(paypalButton.value); // 👈 usar la ref directamente
};

onMounted(() => {
  const script = document.createElement('script');
  script.src =
    'https://www.paypal.com/sdk/js?client-id=AXyaQ4frJzmFwCRK7Nf6_rm66IzaMoQGtjNIpBKwarHewHArcTHJGob_gJqsp2nQVjB18sA-osj0u_QK&currency=USD';
  script.addEventListener('load', setLoaded);
  document.body.appendChild(script);
});
</script>

<template>
  <div>
    <!-- PayPal button -->
    <div ref="paypalButton" class="w-full"></div>
    
    <!-- Mensaje post-pago -->
    <p v-if="paidFor" class="text-green-600 mt-2">¡Pago realizado con éxito!</p>
  </div>
</template>
