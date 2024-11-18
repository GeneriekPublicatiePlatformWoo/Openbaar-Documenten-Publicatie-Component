import { ref, onMounted, type ComputedRef, watchEffect } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import { useAllPages } from "@/composables/use-all-pages";
import toast from "@/stores/toast";
import { uploadFile } from "../service";
import type { PublicatieDocument } from "../types";

const API_URL = `/api/v1`;

export const useDocumenten = (pubUUID: ComputedRef<string | undefined>) => {
  // Documenten
  const files = ref<File[]>([]);
  const documenten = ref<PublicatieDocument[]>([]);

  const loadingDocumenten = ref(false);
  const documentenError = ref(false);

  const getDocumenten = () => {
    const { data, loading, error } = useAllPages<PublicatieDocument>(
      `${API_URL}/documenten/?publicatie=${pubUUID.value}`
    );

    watchEffect(() => {
      documenten.value = data.value || documenten.value;
      loadingDocumenten.value = loading.value;
      documentenError.value = error.value;
    });
  };

  const submitDocumenten = async () => {
    if (!pubUUID.value || !documenten.value) return;

    try {
      for (const [index, doc] of documenten.value.entries()) {
        if (!doc.uuid) {
          docUUID.value = undefined;

          await postDocument({ ...doc, publicatie: pubUUID.value }).execute();

          !documentError.value && (await uploadDocument(index));
        } else {
          docUUID.value = doc.uuid;

          await putDocument(doc).execute();
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

  // Document
  const docUUID = ref<string>();
  const uploadingFile = ref(false);

  const {
    post: postDocument,
    put: putDocument,
    data: documentData,
    isFetching: loadingDocument,
    error: documentError
  } = useFetchApi(() => `${API_URL}/documenten${docUUID.value ? "/" + docUUID.value : ""}`, {
    immediate: false
  }).json<PublicatieDocument>();

  const uploadDocument = async (index: number) => {
    if (files.value?.[index] && documentData.value?.bestandsdelen?.length) {
      uploadingFile.value = true;

      try {
        await uploadFile(files.value[index], documentData.value.bestandsdelen);
      } catch {
        toast.add({
          text: "Het document kon niet worden geupload, probeer het nogmaals...",
          type: "error"
        });

        throw new Error();
      } finally {
        uploadingFile.value = false;
      }
    } else {
      throw new Error();
    }
  };

  onMounted(() => pubUUID.value && getDocumenten());

  return {
    files,
    documenten,
    loadingDocumenten,
    documentenError,
    loadingDocument,
    documentError,
    uploadingFile,
    submitDocumenten
  };
};
