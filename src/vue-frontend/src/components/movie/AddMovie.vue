<template>
  <admin-layout>
    <div class="form">
      <input type="text" v-model="title" placeholder="title"><br/>
      <input type="number" v-model="duration" placeholder="duration"><br/>
      <input type="text" v-model="genre" placeholder="genre"><br/>
      <input type="text" v-model="rating" placeholder="rating">
      <button v-on:click="postMovie">Voeg film toe</button>
    </div>
  </admin-layout>
</template>

<script>
  import AdminLayout from "../../layouts/AdminLayout";
  import {getData, postData} from "../../services/ChromelyBackendService";

  export default {
    name: "AddMovie",
    components: {AdminLayout},
    data() {
      return {
        title: null,
        duration: null,
        genre: null,
        rating: null
      }
    },
    methods: {
      postMovie: function () {
        postData('/movies/add',
          {
            "title": this.title,
            "duration": parseInt(this.duration),
            "genre": this.genre,
            "rating": parseFloat(this.rating),
          }).then((result => {
              this.$router.push('/');
        }))
      }
    }
  }
</script>

<style scoped>

</style>
