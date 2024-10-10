import { ref, onMounted, watch, type ComputedRef } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import { mimeTypesMap, uploadFile } from "../service";
import { PublicatieStatus, type PublicatieDocument } from "../types";

const DOCAPI_URL = `/api/v1/documenten`;

export const useDocumenten = (pubUUID: ComputedRef<string | undefined>) => {
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
    status: PublicatieStatus.gepubliceerd
  });

  // Documenten

  const documenten = ref<PublicatieDocument[]>([]);
  const files = ref<File[]>();

  const {
    get: getDocumenten,
    data: documentenData,
    isFetching: loadingDocumenten,
    error: documentenError
  } = useFetchApi(() => `${DOCAPI_URL}/?publicatie=${pubUUID.value}`, {
    immediate: false
  }).json<PublicatieDocument[]>();

  watch(documentenData, (value) => (documenten.value = value || documenten.value));

  watch(files, () => addDocumenten());

  const addDocumenten = () => {
    const docs: PublicatieDocument[] = [];

    try {
      Array.from(files.value || []).forEach((file) => {
        const doc = getInitialDocument();
        const bestandsformaat = mimeTypesMap.value?.get(file.type)?.identifier;

        if (!bestandsformaat) throw new Error();

        doc.officieleTitel = file.name.replace(/\.[^/.]+$/, ""); // file name minus extension as default title
        doc.bestandsnaam = file.name;
        doc.bestandsformaat = bestandsformaat;
        doc.bestandsomvang = file.size;
        docs.push(doc);
      });
    } catch {
      return;
    }

    documenten.value = [...docs, ...documenten.value];
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
  } = useFetchApi(() => `${DOCAPI_URL}${docUUID.value ? "/" + docUUID.value : ""}`, {
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

  const removeDocument = (index: number) => documenten.value.splice(index, 1);

  onMounted(async () => pubUUID.value && (await getDocumenten().execute()));

  return {
    files,
    documenten,
    loadingDocumenten,
    documentenError,
    loadingDocument,
    documentError,
    uploadingFile,
    submitDocumenten,
    removeDocument
  };
};
