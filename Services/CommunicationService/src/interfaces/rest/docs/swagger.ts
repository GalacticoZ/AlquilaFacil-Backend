import swaggerJSDoc from 'swagger-jsdoc';

const swaggerOptions = {
  definition: {
    openapi: '3.0.0',
    info: {
      title: 'Communication.API',
      version: '1.0.0',
      description: 'API documentation for the Communication service',
      termsOfService: 'https://alquila-facil.com/tos',
      contact: {
        name: 'Alquila Fácil',
        email: 'contact@alquilaf.com'
      },
      license: {
        name: 'Apache 2.0',
        url: 'https://www.apache.org/licenses/LICENSE-2.0.html'
      }
    }
  },
  apis: [__dirname + '/../routes/*.ts'],
}

const swaggerSpec = swaggerJSDoc(swaggerOptions);

export default swaggerSpec;