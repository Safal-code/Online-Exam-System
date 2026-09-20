const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

//RequestInit is built in type for fetch option(methods,body,headers)
interface RequestOptions extends RequestInit {//fetch has no userId option so u define explicitly here
  userId?: number;
}

export async function apiClient<T>(
  endpoint: string,
  options: RequestOptions = {} //here  ={} is default so it can become empty obj if caller pass nothing
): Promise<T> {
  const { userId, ...fetchOptions } = options; //Takes userId out, and puts everything else (method, body...) into fetchOptions.

  const headers = new Headers(fetchOptions.headers);

  //tell my data is json
  headers.set("Content-Type", "application/json");

  //user id passed on every http req.
  if (userId) {
    headers.set("X-User-Id", userId.toString());
  }

  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...fetchOptions,
    headers,
  });

  if (!response.ok) {
    let message = "Something went wrong";

    try {
      const errorData = await response.json();
      message = errorData.message || message;
    } catch {
      // Keep default message
    }

    throw new Error(message);
  }

  return response.json();
}