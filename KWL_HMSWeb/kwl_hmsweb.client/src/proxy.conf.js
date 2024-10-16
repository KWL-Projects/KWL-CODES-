const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7074';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target,
    secure: false
  },
  {
    context: [
      "/api",  // Add the /api context here
    ],
    target: 'https://localhost:7074', // Ensure the API is directed to the backend port
    secure: false
  }
];

module.exports = PROXY_CONFIG;

