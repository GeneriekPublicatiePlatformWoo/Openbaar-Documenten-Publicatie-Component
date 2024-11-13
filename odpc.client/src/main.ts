import "./assets/simple.css";
import "./assets/design-tokens.scss";
import "./assets/main.scss";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import formInvalidHandler from "./directives/form-invalid-handler";

const app = createApp(App);

app.use(router);

app.directive("form-invalid-handler", formInvalidHandler);

app.mount("#app");
