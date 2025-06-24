<script setup>
import { ref } from 'vue';

const faqs = ref([
  {
    question: "¿Cómo reservo un espacio?",
    answer:
      "Primero debes iniciar sesión, seleccionar el espacio deseado y completar la información de reserva (fecha y horario). Luego deberás realizar el depósito al número de cuenta indicado y subir el comprobante de pago para su validación.",
  },
  {
    question: "¿Qué sucede después de subir el comprobante de pago?",
    answer:
      "El propietario del espacio validará el comprobante. Si todo es correcto, la reserva quedará confirmada. En caso de problemas o fraude, el propietario puede rechazarla.",
  },
  {
    question: "¿AlquilaFácil gestiona el dinero o reembolsa pagos?",
    answer:
      "No. AlquilaFácil no retiene ni transfiere dinero. Todos los pagos se hacen directamente al propietario, por lo que es importante verificar bien los datos antes de transferir.",
  },
  {
    question: "¿Cómo activo una suscripción premium?",
    answer:
      "Realiza el pago correspondiente y sube el comprobante. Un administrador validará la información y activará tu suscripción manualmente.",
  },
  {
    question: "¿Qué beneficios tengo como usuario premium?",
    answer:
      "Acceso prioritario a espacios, reservas protegidas que no pueden ser modificadas por el propietario, y funciones adicionales dentro de la plataforma.",
  },
  {
    question: "¿Qué hago si sospecho de un fraude?",
    answer:
      "Puedes reportar el local desde la sección de detalles. El equipo revisará el caso y tomará medidas si corresponde. Reportes falsos pueden llevar a sanciones.",
  },
  {
    question: "¿Puedo posponer una reserva?",
    answer:
      "Sí, pero solo si la reserva no es premium. Las reservas estándar se pueden posponer desde el calendario si están resaltadas en azul en un máximo de una hora.",
  },
]);

const expandedIndex = ref(null);

const form = ref({
  name: '',
  email: '',
  message: '',
});

const sendEmail = () => {
  const subject = encodeURIComponent(`Consulta de ${form.value.name}`);
  const body = encodeURIComponent(`Nombre: ${form.value.name}\nCorreo: ${form.value.email}\n\n${form.value.message}`);
  const mailtoLink = `mailto:soporte@alquilafacil.com?subject=${subject}&body=${body}`;

  window.location.href = mailtoLink;
};

const toggleFaq = (index) => {
  expandedIndex.value = expandedIndex.value === index ? null : index;
};
</script>

<template>
  <div class="px-4 sm:px-6 md:px-12 lg:px-24 py-10 min-h-[80dvh] text-(--text-color)">
    <h3 class="text-xl font-semibold text-center mb-8">Soporte - Preguntas frecuentes</h3>

    <div class="max-w-3xl mx-auto space-y-4">
      <div
        v-for="(faq, index) in faqs"
        :key="index"
        class="border border-gray-300 rounded-md overflow-hidden"
      >
        <button
          @click="toggleFaq(index)"
          class="w-full text-left px-6 py-4 hover:cursor-pointer"
        >
          <span class="font-medium">{{ faq.question }}</span>
        </button>

        <div v-if="expandedIndex === index" class="px-6 py-4 ">
          <p class="text-sm">{{ faq.answer }}</p>
        </div>
      </div>
    </div>

    <div class="mt-10 border-t pt-8 max-w-3xl mx-auto">
      <h4 class="text-lg font-semibold mb-4 text-center">¿Aún tienes dudas?</h4>
      <p class="text-sm text-center mb-4">Envíanos tu consulta y te responderemos lo antes posible.</p>

      <div class="space-y-4 w-full">
        <input
          v-model="form.name"
          type="text"
          required
          placeholder="Tu nombre"
          class="w-full p-4 border border-gray-300 rounded-md"
        />
        <input
          v-model="form.email"
          type="email"
          required
          placeholder="Tu correo"
          class="w-full p-4 border border-gray-300 rounded-md"
        />
        <textarea
          v-model="form.message"
          required
          placeholder="Escribe tu mensaje aquí"
          rows="4"
          class="w-full p-4 border border-gray-300 rounded-md"
        ></textarea>

        <button
          @click="sendEmail"
          class="bg-(--secondary-color) hover:bg-(--secondary-color-hover) text-white font-semibold px-4 py-4 rounded-md transition w-full"
        >
          Enviar mensaje
        </button>
      </div>
    </div>

  </div>
</template>
