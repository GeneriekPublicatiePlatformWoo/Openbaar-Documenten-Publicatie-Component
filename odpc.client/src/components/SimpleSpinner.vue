<template>
  <div aria-live="polite" class="loader">Bezig met laden...</div>
</template>

<style lang="scss" scoped>
// The MIT License (MIT)

// Copyright (c) 2014 Luke Haas

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
.loader {
  font-size: 4rem;
  text-indent: -9999em;
  overflow: hidden;
  width: 1em;
  height: 1em;
  border-radius: 50%;
  margin: 0.8em auto;
  position: relative;
  transform: translateZ(0);
  animation:
    load6 1.7s infinite ease,
    round 1.7s infinite ease;
}

@keyframes load6 {
  0% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      0 -0.83em 0 -0.42em,
      0 -0.83em 0 -0.44em,
      0 -0.83em 0 -0.46em,
      0 -0.83em 0 -0.477em;
  }

  5%,
  95% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      0 -0.83em 0 -0.42em,
      0 -0.83em 0 -0.44em,
      0 -0.83em 0 -0.46em,
      0 -0.83em 0 -0.477em;
  }

  10%,
  59% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      -0.087em -0.825em 0 -0.42em,
      -0.173em -0.812em 0 -0.44em,
      -0.256em -0.789em 0 -0.46em,
      -0.297em -0.775em 0 -0.477em;
  }

  20% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      -0.338em -0.758em 0 -0.42em,
      -0.555em -0.617em 0 -0.44em,
      -0.671em -0.488em 0 -0.46em,
      -0.749em -0.34em 0 -0.477em;
  }

  38% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      -0.377em -0.74em 0 -0.42em,
      -0.645em -0.522em 0 -0.44em,
      -0.775em -0.297em 0 -0.46em,
      -0.82em -0.09em 0 -0.477em;
  }

  100% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      0 -0.83em 0 -0.42em,
      0 -0.83em 0 -0.44em,
      0 -0.83em 0 -0.46em,
      0 -0.83em 0 -0.477em;
  }
}

@keyframes load6 {
  0% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      0 -0.83em 0 -0.42em,
      0 -0.83em 0 -0.44em,
      0 -0.83em 0 -0.46em,
      0 -0.83em 0 -0.477em;
  }

  5%,
  95% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      0 -0.83em 0 -0.42em,
      0 -0.83em 0 -0.44em,
      0 -0.83em 0 -0.46em,
      0 -0.83em 0 -0.477em;
  }

  10%,
  59% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      -0.087em -0.825em 0 -0.42em,
      -0.173em -0.812em 0 -0.44em,
      -0.256em -0.789em 0 -0.46em,
      -0.297em -0.775em 0 -0.477em;
  }

  20% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      -0.338em -0.758em 0 -0.42em,
      -0.555em -0.617em 0 -0.44em,
      -0.671em -0.488em 0 -0.46em,
      -0.749em -0.34em 0 -0.477em;
  }

  38% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      -0.377em -0.74em 0 -0.42em,
      -0.645em -0.522em 0 -0.44em,
      -0.775em -0.297em 0 -0.46em,
      -0.82em -0.09em 0 -0.477em;
  }

  100% {
    box-shadow:
      0 -0.83em 0 -0.4em,
      0 -0.83em 0 -0.42em,
      0 -0.83em 0 -0.44em,
      0 -0.83em 0 -0.46em,
      0 -0.83em 0 -0.477em;
  }
}

@keyframes round {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}

@keyframes round {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}
</style>
