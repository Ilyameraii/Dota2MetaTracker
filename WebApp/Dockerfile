FROM nginx:alpine

COPY WebApp/index.html /usr/share/nginx/html/index.html
COPY WebApp/style.css /usr/share/nginx/html/style.css
COPY WebApp/script.js /usr/share/nginx/html/script.js
COPY WebApp/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
