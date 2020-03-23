<template>
  <CustomerLayout>
    <h2>Dit is component buiten de customer layout: {{ msg }}</h2>
    <router-link to="/admin">Ga naar admin menu</router-link>
  </CustomerLayout>
</template>

<script>
import CustomerLayout from "../layouts/CustomerLayout";
export default {
  name: 'HelloWorld',
  components: {CustomerLayout},
  data () {
    return {
      msg: 'Customer layout'
    }
  }
}
</script>
