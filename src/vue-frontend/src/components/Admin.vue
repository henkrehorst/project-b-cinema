<template>
  <AdminLayout>
    <h2>Dit is de admin layout</h2>
    <p>Chromely backend data: {{ chromelyBackendData }}</p>
    <router-link to="/">Ga terug naar de home page</router-link>
    <div class="form">
      <input v-model="fieldData" placeholder="message">
      <button v-on:click="postField">Verstuur</button>
      <p>{{ postOutput }}</p>
    </div>
  </AdminLayout>
</template>

<script>
  import AdminLayout from "../layouts/AdminLayout";
  import {getData, postData} from "../services/ChromelyBackendService";

  export default {
    name: "Admin",
    components: {AdminLayout},
    data() {
      return {
        chromelyBackendData: null,
        postOutput: null,
        fieldData: null
      }
    },
    mounted() {

      getData('/demo').then((result => {
        this.chromelyBackendData = result.Data
      }))
    },
    methods: {
      postField: function () {
        postData('/demo/post',{"message":this.fieldData}).then((result => {
          this.postOutput = result.Data;
        }))
      }
    }
  }
</script>

<style scoped>
  h2 {
    color: blue;
  }
</style>
