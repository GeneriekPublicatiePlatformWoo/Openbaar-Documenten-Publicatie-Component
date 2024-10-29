export type User = {
  isLoggedIn: boolean;
  id: string;
  fullName: string;
  roles: string[];
  isAdmin: boolean;
};

// cache de gefetchde user, zodat deze niet onnodig opnieuw wordt opgevraagd van de server
let user: User | null = null;

// returns gecachede user
export default async function getUser(useCache: boolean = true) {
  if (useCache && user) {
    return user;
  }

  try {
    user = await fetch("/api/me").then((r) => r.json());

    return user;
  } catch {
    return null;
  }
}
