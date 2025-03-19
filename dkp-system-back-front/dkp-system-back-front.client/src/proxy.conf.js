const PROXY_CONFIG = [
  {
    context: [
      "/events",
    ],
    target: "https://localhost:7239",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
