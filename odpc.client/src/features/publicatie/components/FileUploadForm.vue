<template>
  <fieldset>
    <legend>Document</legend>

    <div class="form-group">
      <label for="titel">Titel</label>

      <input
        id="titel"
        type="text"
        v-model="publicatieDocument.officieleTitel"
        required
        aria-required="true"
      />
    </div>

    <div class="form-group">
      <label for="verkorte_titel">Verkorte titel</label>

      <input id="verkorte_titel" type="text" v-model="publicatieDocument.verkorteTitel" />
    </div>

    <div class="form-group">
      <label for="omschrijving">Omschrijving</label>

      <textarea id="omschrijving" v-model="publicatieDocument.omschrijving" rows="4"></textarea>
    </div>

    <div class="form-group">
      <label for="bestand">Bestand toevoegen</label>

      <input id="bestand" type="file" title="Voeg bestand toe" @change="onFileSelected" />
    </div>

    <!-- <pre>{{ publicatieDocument }}</pre> -->
  </fieldset>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "./mime-types";

type ResponseMessage = "succes" | "error";

defineExpose({ submit });

const file = ref<File | null>();

const publicatieDocument = ref<PublicatieDocument>({
  publicatie: "",
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0],
  bestandsnaam: "",
  bestandsformaat: "",
  bestandsomvang: 0
});

const onFileSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  file.value = target.files[0];

  publicatieDocument.value = {
    ...publicatieDocument.value,
    bestandsnaam: file.value.name,
    bestandsformaat: mimeTypesMap.value?.get(file.value.type)?.identifier,
    bestandsomvang: file.value.size
  };
};

const uploadDocument = async (): Promise<ResponseMessage> => {
  const { data, error: postError } = await useFetchApi(() => `/api-mock/v1/documenten`)
    .post(publicatieDocument.value)
    .json<{ bestandsdelen: { url: string; omvang: number }[] }>();

  // ErrorHandling...
  if (postError.value || !file.value || !data.value?.bestandsdelen.length) return "error";

  let blobStart = 0;

  try {
    for (const { url, omvang } of data.value.bestandsdelen) {
      const body = new FormData();
      const blob = file.value.slice(blobStart, blobStart + omvang);

      blobStart += omvang;

      body.append("inhoud", blob);

      const { error: putError } = await useFetchApi(url).put(body);

      if (putError.value) {
        throw new Error();
      }
    }
  } catch {
    return "error";
  }

  return "succes";
};

async function submit(publicatie: string): Promise<ResponseMessage> {
  publicatieDocument.value = {
    ...publicatieDocument.value,
    publicatie
  };

  return await uploadDocument();
}
</script>

<style lang="scss" scoped></style>
