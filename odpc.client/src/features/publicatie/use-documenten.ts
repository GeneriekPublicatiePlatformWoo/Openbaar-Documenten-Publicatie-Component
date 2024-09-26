import { ref, onMounted, watch, type ComputedRef } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import { uploadFile } from "./service";
import type { PublicatieDocument } from "./types";

export const useDocumenten = (uuid: ComputedRef<string | undefined>) => {
  const files = ref<FileList>();

  // TODO...
  watch(files, () => {
    Array.from(files.value || []).forEach((file) => {
      const doc = getInitialDocument();

      // const bestandsformaat = mimeTypesMap.value?.get(file.type)?.identifier;

      doc.bestandsnaam = file.name;
      doc.bestandsomvang = file.size;

      documenten.value.push(doc);
    });
  });

  const getInitialDocument = (): PublicatieDocument => ({
    publicatie: "",
    officieleTitel: "",
    verkorteTitel: "",
    omschrijving: "",
    creatiedatum: new Date().toISOString().split("T")[0],
    bestandsnaam: "",
    bestandsformaat: "",
    bestandsomvang: 0,
    bestandsdelen: [],
    status: "gepubliceerd"
  });

  const documenten = ref<PublicatieDocument[]>([getInitialDocument()]);

  const {
    get: getDocumenten,
    data: documentenData,
    isFetching: loadingDocumenten,
    error: documentenError
  } = useFetchApi(() => `/api/v1/documenten/?publicatie=${uuid.value}`, {
    immediate: false
  }).json<PublicatieDocument[]>();

  watch(documentenData, (value) => (documenten.value = value || documenten.value), {
    immediate: false
  });

  // TODO...
  const uploadingFiles = ref(false);
  const docSubmitUrl = ref(`/api/v1/documenten`);

  const {
    post: postDocument,
    put: putDocument,
    data: documentData,
    isFetching: loadingDocument,
    error: documentError
  } = useFetchApi(() => docSubmitUrl.value, {
    immediate: false
  }).json<PublicatieDocument>();

  const removeDocument = async (uuid: string) => {
    const doc = documenten.value.find((doc) => doc.uuid === uuid);

    if (doc) doc.status = "ingetrokken"; // ...
  };

  const submitDocumenten = async (): Promise<void> => {
    if (!uuid.value || !documenten.value) return; // ...

    try {
      for (const doc of documenten.value) {
        if (doc.status === "ingetrokken") {
          docSubmitUrl.value = `/api/v1/documenten/${uuid.value}`;

          await putDocument(doc).execute();
        } else if (!doc.uuid) {
          const index = documenten.value
            .filter((d) => !d.uuid)
            .findIndex((d) => d.uuid === doc.uuid); //...

          docSubmitUrl.value = "/api/v1/documenten";

          await postDocument({ ...doc, publicatie: uuid.value }).execute();

          !documentError.value && await uploadDocument(index);
        }

        if (documentError.value) {
          toast.add({
            text: "De metadata bij het document kon niet worden opgeslagen, probeer het nogmaals...",
            type: "error"
          });
    
          throw new Error();
        }
      }
    } catch {
      throw new Error();
    }
  };

  const uploadDocument = async (index: number): Promise<void> => {
    if (files.value?.[index] && documentData.value?.bestandsdelen?.length) {
      uploadingFiles.value = true;

      try {
        await uploadFile(files.value[index], documentData.value.bestandsdelen);
      } catch {
        toast.add({
          text: "Het document kon niet worden geupload, probeer het nogmaals...",
          type: "error"
        });

        throw new Error();
      } finally {
        uploadingFiles.value = false;
      }
    } else {
      throw new Error();
    }
  };

  onMounted(async () => uuid.value && (await getDocumenten().execute()));

  return {
    files,
    documenten,
    loadingDocumenten,
    documentenError,
    loadingDocument,
    documentError,
    uploadingFiles,
    submitDocumenten,
    removeDocument
  };
};
