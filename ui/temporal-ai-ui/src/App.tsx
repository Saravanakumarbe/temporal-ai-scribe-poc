import { useMemo, useState } from 'react';

interface WorkflowProgress {
  workflowId: string;
  currentStep: string;
  percent: number;
  status: string;
}

export default function App() {
  const [workflowId, setWorkflowId] = useState('');
  const [status, setStatus] = useState('Idle');
  const [progress, setProgress] = useState<WorkflowProgress | null>(null);
  const [audioUrl, setAudioUrl] = useState('sample.mp3');

  const startWorkflow = async () => {
    setStatus('Starting');
    const response = await fetch('/api/workflows', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ audioUrl }),
    });
    const payload = await response.json();
    setWorkflowId(payload.workflowId);
    setStatus(payload.status);
  };

  const refreshProgress = async () => {
    if (!workflowId) return;
    const response = await fetch(`/api/workflows/${workflowId}`);
    const payload = await response.json();
    setProgress(payload);
    setStatus(payload.status);
  };

  const review = async (action: 'approve' | 'reject' | 'cancel') => {
    if (!workflowId) return;
    await fetch(`/api/workflows/${workflowId}/${action}`, { method: 'POST' });
    await refreshProgress();
  };

  const percent = useMemo(() => progress?.percent ?? 0, [progress]);

  return (
    <main style={{ fontFamily: 'sans-serif', padding: '2rem', maxWidth: '900px', margin: '0 auto' }}>
      <h1>Temporal AI Scribe Dashboard</h1>
      <p>Start an audio workflow, watch its progress, and approve or reject the generated SOAP note.</p>

      <div style={{ display: 'grid', gap: '0.75rem', marginTop: '1.5rem' }}>
        <label>
          Audio URL
          <input value={audioUrl} onChange={(event) => setAudioUrl(event.target.value)} style={{ display: 'block', width: '100%', marginTop: '0.25rem' }} />
        </label>
        <button onClick={startWorkflow}>Start Workflow</button>
        <button onClick={refreshProgress} disabled={!workflowId}>Refresh Status</button>
      </div>

      <section style={{ marginTop: '2rem' }}>
        <h2>Status</h2>
        <p>Workflow ID: {workflowId || '—'}</p>
        <p>Current state: {status}</p>
        <div style={{ background: '#f4f4f4', padding: '1rem', borderRadius: '8px' }}>
          <div style={{ height: '12px', width: '100%', background: '#d9d9d9', borderRadius: '999px', overflow: 'hidden' }}>
            <div style={{ height: '100%', width: `${percent}%`, background: '#2563eb' }} />
          </div>
          <p style={{ marginTop: '0.5rem' }}>{progress?.currentStep ?? 'Queued'}</p>
        </div>
      </section>

      <section style={{ marginTop: '2rem' }}>
        <h2>Review</h2>
        <div style={{ display: 'flex', gap: '0.5rem' }}>
          <button onClick={() => review('approve')} disabled={!workflowId}>Approve</button>
          <button onClick={() => review('reject')} disabled={!workflowId}>Reject</button>
          <button onClick={() => review('cancel')} disabled={!workflowId}>Cancel</button>
        </div>
      </section>
    </main>
  );
}
