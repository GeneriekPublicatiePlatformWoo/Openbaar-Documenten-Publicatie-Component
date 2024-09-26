import { ref, onMounted, watch, type ComputedRef } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import { uploadFile } from "./service";
import type { PublicatieDocument } from "./types";

export const useDocumenten = (uuid: ComputedRef<string | undefined>) => {
  const files = ref<File[]>([]);

  const uploadingDocument = ref(false);

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

  watch(
    documentenData,
    (value) => {
      documenten.value = value || documenten.value;
      !documenten.value.length && addDocument();
    },
    {
      immediate: false
    }
  );

  // TODO...

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

  const addDocument = (): void => {
    documenten.value.push(getInitialDocument());
  };

  const removeDocument = async (uuid: string) => {
    const doc = documenten.value.find((doc) => doc.uuid === uuid);

    docSubmitUrl.value = `/api/v1/documenten/${uuid}`;

    // Loading / Error ...
    await putDocument({ ...doc, status: "ingetrokken" }).execute();

    await getDocumenten().execute();
  };

  const submitDocument = async (): Promise<void> => {
    if (!uuid.value || !documenten.value || documenten.value[0].uuid) return;

    documenten.value[0].publicatie = uuid.value;

    docSubmitUrl.value = "/api/v1/documenten";

    await postDocument(documenten.value[0]).execute();

    if (documentError.value) {
      toast.add({
        text: "De metadata bij het document kon niet worden opgeslagen, probeer het nogmaals...",
        type: "error"
      });

      throw new Error();
    }
  };

  const uploadDocument = async (): Promise<void> => {
    if (files.value && documentData.value?.uuid && documentData.value?.bestandsdelen?.length) {
      uploadingDocument.value = true;

      try {
        await uploadFile(files.value[0], documentData.value.bestandsdelen);
      } catch {
        toast.add({
          text: "Het document kon niet worden geupload, probeer het nogmaals...",
          type: "error"
        });

        removeDocument(documentData.value.uuid);

        throw new Error();
      } finally {
        uploadingDocument.value = false;
      }
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
    uploadingDocument,
    submitDocument,
    uploadDocument,
    addDocument,
    removeDocument
  };
};
