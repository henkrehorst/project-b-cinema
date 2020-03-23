<template>
  <CustomerLayout>
    <h2>Dit is component buiten de customer layout: {{ msg }}</h2>
    <router-link to="/admin">Ga naar admin menu</router-link>
    <p>Backend message: {{ backendMessage }}</p>
  </CustomerLayout>
</template>

<script>
  import CustomerLayout from "../layouts/CustomerLayout";
  import {getData} from "../services/ChromelyBackendService";

  export default {
    name: 'HelloWorld',
    components: {CustomerLayout},
    data() {
      return {
        msg: 'Customer layout',
        backendMessage: null
      }
    },
    mounted() {
      getData("/demo").then((result => {
        this.backendMessage = result.Data;
      }))
    }
  }
</script>
