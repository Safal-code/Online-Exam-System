import { useEffect, useState } from "react";

import { useAuth } from "./useAuth";
import { getPublishedTests } from "../services/testService";

import type { PublishedTest } from "../types/test";

export function useTests() {
  const { user } = useAuth();

  const [tests, setTests] =
    useState<PublishedTest[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState("");

  useEffect(() => {
    if (!user) {
      setLoading(false);
      return;
    }

    async function loadTests() {
      if (!user) {
        setLoading(false);
        return;
      }
      try {
        const data = await getPublishedTests(
          user.userId
        );

        setTests(data);
      } catch (error) {
        setError(
          error instanceof Error
            ? error.message
            : "Failed to load tests"
        );
      } finally {
        setLoading(false);
      }
    }

    loadTests();
  }, [user]);

  return {
    tests,
    loading,
    error,
  };
}