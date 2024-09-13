import { ref } from "vue";

export type User = {
  isLoggedIn: boolean;
  id: string;
  fullName: string;
  roles: string[];
};

// cache de gefetchde user, zodat deze niet onnodig opnieuw wordt opgevraagd van de server
const user = ref<User | null>(null);

// returns gecachede user
export default async function getUser(useCache: boolean = true) {
  if (useCache && user.value) {
    return user.value;
  }

  try {
    user.value = await fetch("/api/me").then((r) => r.json());

    return user.value;
  } catch {
    return null;
  }
}
