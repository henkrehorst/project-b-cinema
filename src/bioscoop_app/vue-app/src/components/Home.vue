<template>
  <CustomerLayout>
    <h2>Dit is component buiten de customer layout: {{ msg }}</h2>
    <router-link to="/admin">Ga naar admin menu</router-link>
    <h3>Films</h3>
    <table>
      <tr>
        <th>Titel</th>
        <th>Genre</th>
        <th>Rating</th>
        <th>Duur</th>
      </tr>
      <tr v-for="movie in movies" :key="movie.id">
        <td>{{movie.title}}</td>
        <td>{{movie.genre}}</td>
        <td>{{movie.rating}}</td>
        <td>{{movie.duration}}</td>
      </tr>
    </table>
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
        movies: null
      }
    },
    mounted() {
      getData("/movies").then((result => {
        this.movies = JSON.parse(result.Data);
      }));
    }
  }
</script>
<style>
  table {
    font-family: arial, sans-serif;
    border-collapse: collapse;
    width: 100%;
  }

  td, th {
    border: 1px solid #dddddd;
    text-align: left;
    padding: 8px;
  }

  tr:nth-child(even) {
    background-color: #dddddd;
  }
</style>
