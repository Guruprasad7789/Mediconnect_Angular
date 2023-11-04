const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/home",
      "/donations",
      "/recipient",
      "/donor",
      "/login",
      "/feedback",
      "/contact",
      "/hospital",
    ],
    target: "https://localhost:7194",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
