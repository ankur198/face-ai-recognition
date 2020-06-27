<template>
  <div class="container">
    <div>
      <Logo />
      <h1 class="title">
        Mask Immune Face Recognition System
      </h1>
      <div class="body">
        <div class="input">
          <input
            type="file"
            @change="previewFiles"
            accept=".jpg, .png, .jpeg"
          />
          <img :src="file" alt="" class="preview" />
        </div>
        <div v-if="output.length !== 0">
          <h1>Our Prediction:</h1>
          <div
            class="prediction"
            v-for="(prediction, index) in output"
            :key="index"
          >
            <div class="name">{{ prediction.name }}</div>
            <div class="probability">
              {{ (prediction.probability * 100).toFixed(2) }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      output: [],
      file: '',
    }
  },
  methods: {
    async previewFiles(event) {
      if (event.target.files.length === 0) {
        return
      }
      console.log(event.target.files)
      const toBase64 = (file) =>
        new Promise((resolve, reject) => {
          const reader = new FileReader()
          reader.readAsDataURL(file)
          reader.onload = () => resolve(reader.result)
          reader.onerror = (error) => reject(error)
        })
      const base64String = await toBase64(event.target.files[0])
      this.file = base64String
      const formattedBase64 = base64String.substring(
        base64String.indexOf(',') + 1
      )

      // console.log(await this.$axios.$get('/api/face'))

      const response = await this.$axios.$post(`/api/face`, {
        base64Image: formattedBase64,
      })

      const predictedValue = response.predictions
        .filter((x) => x.probability > 0.75)
        .map((x) => {
          return { probability: x.probability, name: x.tagName }
        })
      while(this.output.length!=0){
        this.output.pop()
      }
      predictedValue.forEach((x) => this.output.push(x))
    },
  },
}
</script>

<style>
.container {
  background: #91cfe8;
  margin: 0 auto;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: stretch;
  text-align: center;
  color: #35495e;
}

.title {
  font-family: 'Quicksand', 'Source Sans Pro', -apple-system, BlinkMacSystemFont,
    'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
  display: block;
  font-weight: 900;
  font-size: 50px;
  letter-spacing: 1px;
}

.body {
  margin-top: 50px;
}

.body * {
  padding: 10px;
}

.input {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.input img {
  width: 130px;
  height: auto;
}

.prediction {
  display: flex;
  align-items: center;
  justify-content: center;
}

.prediction .name {
  font-weight: 500;
  font-size: 1.2em;
}

.prediction .probability::after {
  content: '%';
}
</style>
